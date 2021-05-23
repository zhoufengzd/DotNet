using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

using System.IO;
using System.Reflection;

using CT.ABB.Config;


namespace CTS.AGS.Indexer.Configuration
{
    /// <summary>
    /// Provides centralized access to indexer configurations.
    /// </summary>
    public sealed class ConfigMgr
    {
		public static ServiceSettings GetServiceSettings()
		{
			string settingsPath = ConfigurationManager.AppSettings["ServiceSettings"];
			return GetServiceSettings(settingsPath);
		}

        public static ServiceSettings GetServiceSettings(string filePath)
        {
            lock (_syncObject)
            {
				ServiceSettings settings = null;

				// Read connection strings
				string completeFilePath = BuildFilePath( filePath );
				if (File.Exists( completeFilePath ))
					settings = ServiceSettingserializer.LoadFromFile( completeFilePath );
				else
					settings = new ServiceSettings();

#if !(SERVICE_DEBUG)
				try
				{
					settings.CaseVaultConnect = MyConfig.AppSettings.GetItem( "Indexer", "CaseVaultConnect" );
					settings.FunctionalConfigurationURL = MyConfig.AppSettings.GetItem( "Indexer", "FunctionalConfigurationURL" );
					settings.CaseAdministratorURL = MyConfig.AppSettings.GetItem( "Indexer", "CaseAdministratorURL" );
					settings.ClientAdministratorURL = MyConfig.AppSettings.GetItem( "Indexer", "ClientAdministratorURL" );
				}
				catch(Exception e)
				{
					Debug.WriteLine( e.Message );
				}

				wsFuncConfigAdministrator admin = new wsFuncConfigAdministrator();
				admin.Url = settings.FunctionalConfigurationURL;

				try
				{
					FuncConfigNotification funcConfig = admin.GetFuncConfigSettings( FuncConfigSystemEnum.indexer );
					if (funcConfig.Success == true)
					{
						DataRow[] rows = funcConfig.ReturnTable.Select( "name = 'number_of_threads'" );
						if (rows.GetLength( 0 ) != 0)
						{
							settings.NumberOfThreads = Int32.Parse( rows[0]["value"].ToString() );
						}
						rows = funcConfig.ReturnTable.Select( "name = 'high_priority_job_interupt_interval'" );
						if (rows.GetLength( 0 ) != 0)
						{
							settings.IndexingJobCallbackIntervals = Int32.Parse( rows[0]["value"].ToString() );
						}
						rows = funcConfig.ReturnTable.Select( "name = 'SQL_command_timeout'" );
						if (rows.GetLength( 0 ) != 0)
						{
							settings.SqlCommandTimeout = Int32.Parse( rows[0]["value"].ToString() );
						}
						rows = funcConfig.ReturnTable.Select( "name = 'index_job_timeout'" );
						if (rows.GetLength( 0 ) != 0)
						{
							settings.JobTimeout = Int32.Parse( rows[0]["value"].ToString() );
						}
						rows = funcConfig.ReturnTable.Select( "name = 'append_to_index_path'" );
						if (rows.GetLength( 0 ) != 0 )
						{
							settings.AppendToIndexPath = rows[0]["value"].ToString();
						}
						rows = funcConfig.ReturnTable.Select( "name = 'append_to_note_index_path'" );
						if (rows.GetLength( 0 ) != 0)
						{
							settings.AppendToNoteIndexPath = rows[0]["value"].ToString();
						}
                        rows = funcConfig.ReturnTable.Select("name = 'records_per_pass'");
                        if (rows.GetLength(0) != 0)
                        {
                            settings.RecordsPerPass = Int32.Parse(rows[0]["value"].ToString());
                        }

						rows = funcConfig.ReturnTable.Select("name = 'auto_commit_interval_mb'");
						if (rows.GetLength(0) != 0)
						{
							settings.AutoCommitIntervalMB = Int32.Parse(rows[0]["value"].ToString());
						}

						rows = funcConfig.ReturnTable.Select("name = 'max_mem_to_use_mb'");
						if (rows.GetLength(0) != 0)
						{
							settings.MaxMemToUseMB = Int32.Parse(rows[0]["value"].ToString());
						}

                        rows = funcConfig.ReturnTable.Select("name = 'autoindexer_highpriority_recordcount'");
                        if (rows.GetLength(0) != 0)
                        {
                            settings.AutoIndexerHighPriorityRecordCount = Int32.Parse(rows[0]["value"].ToString());
                        }

					}
				}
				catch( Exception e )
				{
					// Can't find service, use settings in indexer.config or defaults
					Debug.WriteLine( e.Message );
				}
#endif

				return settings;
            }
        }

        public static void SaveServiceSettings(string filePath, ref ServiceSettings settings)
        {
            lock (_syncObject)
            {
				string completeFilePath = BuildFilePath( "Indexer.config" );
				if (File.Exists( completeFilePath ))
					ServiceSettingserializer.SaveToFile( completeFilePath, settings );


				wsFuncConfigAdministrator admin = new wsFuncConfigAdministrator();
				admin.Url = settings.FunctionalConfigurationURL;

				try
				{
					string configID_NumberOfThreads = string.Empty;
					string configID_IndexingJobCallbackIntervals = string.Empty;
					string configID_SqlCommandTimeout = string.Empty;
					string configID_IndexJobTimeout = string.Empty;
					string configID_AppendToIndexPath = string.Empty;
					string configID_AppendToNoteIndexPath = string.Empty;

					FuncConfigNotification funcConfig = admin.GetFuncConfigSettings( FuncConfigSystemEnum.indexer );
					if (funcConfig.Success == true)
					{
						DataRow[] rows = funcConfig.ReturnTable.Select( "name = 'number_of_threads'" );
						if (rows.GetLength( 0 ) != 0)
						{
							configID_NumberOfThreads = rows[0]["cmn_configurationID"].ToString();
						}
						rows = funcConfig.ReturnTable.Select( "name = 'high_priority_job_interupt_interval'" );
						if (rows.GetLength( 0 ) != 0)
						{
							configID_IndexingJobCallbackIntervals = rows[0]["cmn_configurationID"].ToString();
						}
						rows = funcConfig.ReturnTable.Select( "name = 'SQL_command_timeout'" );
						if (rows.GetLength( 0 ) != 0)
						{
							configID_SqlCommandTimeout = rows[0]["cmn_configurationID"].ToString();
						}
						rows = funcConfig.ReturnTable.Select( "name = 'index_job_timeout'" );
						if (rows.GetLength( 0 ) != 0)
						{
							configID_IndexJobTimeout = rows[0]["cmn_configurationID"].ToString();
						}
						rows = funcConfig.ReturnTable.Select( "name = 'append_to_index_path'" );
						if (rows.GetLength( 0 ) != 0)
						{
							configID_AppendToIndexPath = rows[0]["cmn_configurationID"].ToString();
						}
						rows = funcConfig.ReturnTable.Select( "name = 'append_to_note_index_path'" );
						if (rows.GetLength( 0 ) != 0)
						{
							configID_AppendToNoteIndexPath = rows[0]["cmn_configurationID"].ToString();
						}

						string[][] settingsArray = new string[][] { new string[] { configID_NumberOfThreads, settings.NumberOfThreads.ToString() }
							, new string[] { configID_IndexingJobCallbackIntervals, settings.IndexingJobCallbackIntervals.ToString() }
							, new string[] { configID_SqlCommandTimeout, settings.SqlCommandTimeout.ToString() }
							, new string[] { configID_IndexJobTimeout, settings.JobTimeout.ToString() }
							, new string[] { configID_AppendToIndexPath, settings.AppendToIndexPath.ToString() }
							, new string[] { configID_AppendToNoteIndexPath, settings.AppendToNoteIndexPath.ToString() } };

						admin.SaveFuncConfigSettings( settingsArray );
					}
				}
				catch (Exception e)
				{
					// Can't find service, use settings in indexer.config or defaults
					Debug.WriteLine( e.Message );
				}
			}
        }

        #region private functions
        private static string BuildFilePath(string filePath)
        {
			string strAppDir = string.Empty;
			string completePath = ConfigurationManager.AppSettings["ServiceSettings"];
			if (String.IsNullOrEmpty( completePath ) )
			{
				completePath = filePath;
			}
			if( !Path.IsPathRooted( completePath ) )
			{
				try
				{
					strAppDir = Path.GetDirectoryName( Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName );
					completePath = Path.Combine( strAppDir, filePath );
				}
				catch (Exception e)
				{
					Debug.WriteLine( e.Message );
				}
			}
			return completePath;
        }
        #endregion

        private static readonly object _syncObject = new object();
    }
}
