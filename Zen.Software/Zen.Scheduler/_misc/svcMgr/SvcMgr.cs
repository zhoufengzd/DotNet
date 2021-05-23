using System;
using System.Collections;
using System.ServiceProcess;
using System.Security.Principal;
using System.Runtime.InteropServices;

namespace Service {

	/// <author>Haidong Chen ( haidong_chen2003@yahoo.com )</author>
	/// <created>05/23/2003</created>
	/// <summary>
	/// Service Manager provide command line access to services 
	/// running on local or remote computers.
	///
	/// Following actions are supported:
	/// 1. Listing brief information on all available services
	/// 2. Listing detailed information on a service
	/// 3. Stopping a service
	/// 4. Starting a service
	/// 5. Restarting a service
	/// 
	/// Online References
	///
	/// Monitoring Windows Services
	/// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/vbcon/html/vborimonitoringwindowsservices.asp
	///
	/// Implementing Impersonation in an ASP.NET Application
	/// http://support.microsoft.com/default.aspx?scid=KB;EN-US;Q306158&"%20Target="_blank
	/// </summary>
	class SvcMgr {

		[STAThread]
		static void Main( string[] args ) {
			try {
				if ( 0 == args.Length ) Console.WriteLine( Instruction );					
				else Process( args );

			} catch( Exception e ) {
				Console.Error.WriteLine( e.Message );
			}
		}

		/// <summary>
		/// Process user command.
		/// </summary>
		/// <param name="args">Command arguments.</param>
		public static void Process( string[] args ) {			
			
			// if only machine name provided ( with no flag ) 
			// or machine name + /STATUS flag provided, get all services on that computer
			if ( args.Length == 1 || args[ 1 ].ToUpper().Equals( Args.STATUS ) ) {
				GetServices( args[ 0 ] );
			
			// if service name is provided, try to manage that service
			} else if ( ! args[ 1 ].StartsWith( "/" ) ) {							

				string user = null, password = null, domain = null;
				int timeout = int.MinValue;

				for( int i = 3; i < args.Length; i++ ) {

					// handle timeout argument					
					if ( args[ i ].ToUpper().StartsWith( Args.TIMEOUT ) ) {
						try {
							timeout = int.Parse( args[ 3 ].Substring( Args.TIMEOUT.Length + 1) );
						} catch( FormatException e ) {
							throw new ArgumentException( "Invalid timeout argument", e );
						}
					
					// handle user name and password argument
					} else if ( args[ i ].ToUpper().StartsWith( Args.USER ) ) {
						user = args[ i ].Substring( Args.USER.Length + 1 );
					} else if ( args[ i ].ToUpper().StartsWith( Args.PASSWORD ) ) {
						password = args[ i ].Substring( Args.PASSWORD.Length + 1 );
					} else if ( args[ i ].ToUpper().StartsWith( Args.DOMAIN ) ) {
						domain = args[ i ].Substring( Args.DOMAIN.Length + 1 );
					}
				}

				// impersonate if needs to
				if ( user != null && password != null && domain != null ) {
					if ( ! ImpersonationUtil.Impersonate( user, password, domain ) ) {
						Console.WriteLine( "No such account found, Impersonation failed." );
						return;
					}
				}

				try {
					ServiceController service = new ServiceController(  args[ 1 ], args[ 0 ] );

					// handle action argument
					if ( args.Length == 2 || args[ 2 ].ToUpper().Equals( Args.STATUS ) ) {
						GetService( service );
					} else if ( args[ 2 ].ToUpper().Equals( Args.RESTART ) ) {
						RestartService( service, timeout );
					} else if ( args[ 2 ].ToUpper().Equals( Args.START ) ) {
						StartService( service, timeout );
					} else if ( args[ 2 ].ToUpper().Equals( Args.STOP ) ) {
						StopService( service, timeout );
					} else if ( args[ 2 ].ToUpper().Equals( Args.PAUSE ) ) {
						PauseService( service, timeout );
					} else if ( args[ 2 ].ToUpper().Equals( Args.CONTINUE ) ) {
						ContinueService( service, timeout );
					} else {
						throw new ArgumentException( "No such action : " + args[ 2 ] );
					}
				
				} finally {

					// undo impersonation 
					if ( user != null && password != null && domain != null ) {
						ImpersonationUtil.UnImpersonate();
					}
				}
			
			} else { GetInstruction(); }
		}

		/// <summary>
		/// Print detailed information on a particular service.
		/// </summary>
		/// <param name="service">Service controller</param>
		private static void GetService( ServiceController service ) {
			string serviceName = service.ServiceName;
			string displayName = service.DisplayName;
			ServiceControllerStatus status = service.Status;

			// print out detail service information
			Console.WriteLine( string.Format("Service Name                 : {0}", serviceName) );
			Console.WriteLine( string.Format("Display Name                 : {0}", displayName) );
			Console.WriteLine( string.Format("Service Status               : {0}", status.ToString()) );
			Console.WriteLine( string.Format("Service Type                 : {0}", service.ServiceType.ToString()) );
			Console.WriteLine( string.Format("Service Can Stop             : {0}", service.CanStop) );
			Console.WriteLine( string.Format("Service Can Pause / Continue : {0}", service.CanPauseAndContinue) );
			Console.WriteLine( string.Format("Service Can Shutdown         : {0}", service.CanShutdown) );
			
			// print out dependent services
			ServiceController[] dependedServices = service.DependentServices;
			Console.Write( string.Format("{0} Depended Service(s)        : ", dependedServices.Length.ToString()) );

			int pos = 0;
			foreach( ServiceController dService in dependedServices ) {
				Console.Write( string.Format("{0}{1}", 
					( ( dependedServices.Length > 1 && pos > 0 )? ", " : string.Empty ), dService.ServiceName) );

				pos ++;
			}

			Console.WriteLine();
		}

		/// <summary>
		/// Get all available services on a computer.
		/// </summary>
		/// <param name="cName">Computer name.</param>
		private static void GetServices( string cName ) {
			ServiceController[] services = ServiceController.GetServices( cName );
			foreach( ServiceController service in services ) {
				Console.WriteLine( string.Format( "{0} [ {1} ]", 
					service.ServiceName, service.Status.ToString() ) );
			}
		}

		/// <summary>
		/// Restart a service.
		/// This action will in turn call StopService and StartService.
		/// If the service is not currently stopped, it will try to stop the service first.
		/// </summary>
		/// <param name="service">service controller.</param>
		/// <param name="timeout">timeout value used for stopping and restarting.</param>
		private static void RestartService( ServiceController service, int timeout ) {
			if ( ServiceControllerStatus.Stopped != service.Status ) {
				StopService( service, timeout );
			}

			StartService( service, timeout );
		}

		/// <summary>
		/// Start a service.
		/// </summary>
		/// <param name="service">service controller.</param>
		/// <param name="timeout">timeout value for starting.</param>
		private static void StartService( ServiceController service, int timeout ) {
			if ( ServiceControllerStatus.Stopped == service.Status ) {
				
				Console.WriteLine( "Starting service '{0}' on '{1}' ...", 
					service.ServiceName, service.MachineName );

				service.Start();

				if ( int.MinValue != timeout ) {
					TimeSpan t = TimeSpan.FromSeconds( timeout );
					service.WaitForStatus( ServiceControllerStatus.Running, t );
				
				} else service.WaitForStatus( ServiceControllerStatus.Running );

				Console.WriteLine( "Started service '{0}' on '{1}'\r\n", 
					service.ServiceName, service.MachineName );
			
			} else {
				Console.WriteLine( "Can not start service '{0}' on '{1}'", 
					service.ServiceName, service.MachineName );

				Console.WriteLine( "Service State '{0}'", service.Status.ToString() );
			}
		}

		/// <summary>
		/// Pause a service.
		/// </summary>
		/// <param name="service">service controller.</param>
		/// <param name="timeout">timeout value for pausing.</param>
		private static void PauseService( ServiceController service, int timeout ) {
			if ( service.CanPauseAndContinue ) {

				Console.WriteLine( "Pausing service '{0}' on '{1}' ...", 
					service.ServiceName, service.MachineName );

				service.Pause();

				if ( int.MinValue != timeout ) {
					TimeSpan t = TimeSpan.FromSeconds( timeout );
					service.WaitForStatus( ServiceControllerStatus.Paused, t );
				
				} else service.WaitForStatus( ServiceControllerStatus.Paused );

				Console.WriteLine( "Paused service '{0}' on '{1}'\r\n", 
					service.ServiceName, service.MachineName );
			
			} else {
				Console.WriteLine( "Can not pause service '{0}' on '{1}'", 
					service.ServiceName, service.MachineName );

				Console.WriteLine( "Service State '{0}'", service.Status.ToString() );
			}
		}

		/// <summary>
		/// Continue a service.
		/// </summary>
		/// <param name="service">service controller.</param>
		/// <param name="timeout">timeout value for continuing.</param>
		private static void ContinueService( ServiceController service, int timeout ) {
			if ( service.CanPauseAndContinue ) {
				Console.WriteLine( "Continuing service '{0}' on '{1}' ...", 
					service.ServiceName, service.MachineName );

				service.Continue();

				if ( int.MinValue != timeout ) {
					TimeSpan t = TimeSpan.FromSeconds( timeout );
					service.WaitForStatus( ServiceControllerStatus.Running, t );
				
				} else service.WaitForStatus( ServiceControllerStatus.Running );

				Console.WriteLine( "Continued service '{0}' on '{1}'\r\n", 
					service.ServiceName, service.MachineName );
			
			} else {
				Console.WriteLine( "Can not continue service '{0}' on '{1}'", 
					service.ServiceName, service.MachineName );

				Console.WriteLine( "Service State '{0}'", service.Status.ToString() );
			}
		}

		/// <summary>
		/// Stop a service.
		/// </summary>
		/// <param name="service">service controller.</param>
		/// <param name="timeout">timeout for stopping the service.</param>
		private static void StopService( ServiceController service, int timeout ) {
			if ( service.CanStop ) {
				Console.WriteLine( "Stopping service '{0}' on '{1}' ...", 
					service.ServiceName, service.MachineName );

				service.Stop();

				if ( int.MinValue != timeout ) {
					TimeSpan t = TimeSpan.FromSeconds( timeout );
					service.WaitForStatus( ServiceControllerStatus.Stopped, t );
				
				} else service.WaitForStatus( ServiceControllerStatus.Stopped );

				Console.WriteLine( "Stopped service '{0}' on '{1}'\r\n", 
					service.ServiceName, service.MachineName );

			} else {
				Console.WriteLine( "Can not stop service '{0}' on '{1}'", 
					service.ServiceName, service.MachineName );

				Console.WriteLine( "Service State '{0}'", service.Status.ToString() );
			}
		}

		public static void GetInstruction() {
			Console.WriteLine( Instruction );
		}

		public static readonly string Instruction = "SVCMGR.EXE 1.0 (c) Haidong Chen ( haidong_chen2003@yahoo.com ). 2003\r\n\r\n"
			+ "Usage:\r\n"
			+ "svcmgr [ computer name ] [ service name ] [ action ] [ additional flags ]\r\n\r\n"
			+ Args.STATUS + "            Display the status of one or all service(s).\r\n"
			+ Args.RESTART + "           Restart a service.\r\n"
			+ Args.STOP + "              Stop a service.\r\n"
			+ Args.START + "             Start a service.\r\n"
			+ Args.PAUSE + "             Pause a service.\r\n"
			+ Args.CONTINUE + "          Continue a service.\r\n"
			+ Args.TIMEOUT + "[:value]   Specify a timeout (seconds), infinite if no timeout specified.\r\n"
			+ Args.USER + "[:value]      Specify user name.\r\n"
			+ Args.PASSWORD + "[:value]  Specify user password.\r\n"
			+ Args.DOMAIN + "[:value]    Specify user domain.";
	}

	public class Args {
		public const string STATUS		= "/STATUS";
		public const string RESTART		= "/RESTART";
		public const string STOP		= "/STOP";
		public const string START		= "/START";
		public const string PAUSE		= "/PAUSE";
		public const string CONTINUE	= "/CONTINUE";
		public const string TIMEOUT		= "/TIMEOUT";
		public const string USER		= "/USER";
		public const string PASSWORD	= "/PASSWORD";
		public const string DOMAIN		= "/DOMAIN";
	}

	/// <summary>
	/// Impersonate a windows logon.
	/// </summary>
	public class ImpersonationUtil {

		/// <summary>
		/// Impersonate given logon information.
		/// </summary>
		/// <param name="logon">Windows logon name.</param>
		/// <param name="password">password</param>
		/// <param name="domain">domain name</param>
		/// <returns></returns>
		public static bool Impersonate( string logon, string password, string domain ) {
			WindowsIdentity tempWindowsIdentity;
			IntPtr token = IntPtr.Zero;
			IntPtr tokenDuplicate = IntPtr.Zero;

			if( LogonUser( logon, domain, password, LOGON32_LOGON_INTERACTIVE, 
					LOGON32_PROVIDER_DEFAULT, ref token) != 0 ) {

				if ( DuplicateToken( token, 2, ref tokenDuplicate ) != 0 ) {
					tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
					impersonationContext = tempWindowsIdentity.Impersonate();
					if ( null != impersonationContext ) return true;				
				}			
			}

			return false;
		}

		/// <summary>
		/// Unimpersonate.
		/// </summary>
		public static void UnImpersonate() {
			impersonationContext.Undo();
		} 

		[DllImport("advapi32.dll", CharSet=CharSet.Auto)]
		public static extern int LogonUser( 
			string lpszUserName, 
			String lpszDomain,
			String lpszPassword,
			int dwLogonType, 
			int dwLogonProvider,
			ref IntPtr phToken );

		[DllImport("advapi32.dll", CharSet=System.Runtime.InteropServices.CharSet.Auto, SetLastError=true)]
		public extern static int DuplicateToken(
			IntPtr hToken, 
			int impersonationLevel,  
			ref IntPtr hNewToken );

		private const int LOGON32_LOGON_INTERACTIVE = 2;
		private const int LOGON32_LOGON_NETWORK_CLEARTEXT = 4;
		private const int LOGON32_PROVIDER_DEFAULT = 0;
		private static WindowsImpersonationContext impersonationContext; 
	}

}
