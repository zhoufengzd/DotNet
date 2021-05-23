using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace _TestProject
{
    internal class CompositeTypeX
    {
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public Int32? Age
        {
            get { return _age; }
            set { _age = value; }
        }
        public DateTime? DateOfBirth
        {
            get { return _dateOfBirth; }
            set { _dateOfBirth = value; }
        }
        public DateTime TodaysDate
        {
            get { return _todaysDate; }
            set { _todaysDate = value; }
        }
        public string[] Contacts
        {
            get { return _contacts; }
            set { _contacts = value; }
        }

        #region private data
        private string _name = "Gorge Williams";
        private Int32? _age;
        private DateTime? _dateOfBirth = DateTime.Today;
        private DateTime _todaysDate;

        private string[] _contacts = new string[] { "Alice", "Tom", "Joan" };
        #endregion
    }

    // Define an example interface.
    public interface ITestArgumentX { }

    // Define an example base class.
    public class TestBaseX { }

    // Define a generic class with one parameter. The parameter
    // has three constraints: It must inherit TestBase, it must
    // implement ITestArgument, and it must have a parameterless
    // constructor.
    public class TestX<T> where T : TestBaseX, ITestArgumentX, new() { }

    // Define a class that meets the constraints on the type
    // parameter of class Test.
    public class TestArgumentX : TestBaseX, ITestArgumentX
    {
        public TestArgumentX() { }
    }

    [TestClass()]
    public class ReflectionTest2 : TestBase
    {
        [TestMethod()]
        public void GenericsTest()
        {
            StringBuilder buffer = new StringBuilder();
            // Two ways to get a Type object that represents the generic
            // type definition of the Dictionary class. 
            //
            // Use the typeof operator to create the generic type 
            // definition directly. To specify the generic type definition,
            // omit the type arguments but retain the comma that separates
            // them.
            Type d1 = typeof(Dictionary<,>);

            // You can also obtain the generic type definition from a
            // constructed class. In this case, the constructed class
            // is a dictionary of Example objects, with String keys.
            Dictionary<string, ReflectionTest2> d2 = new Dictionary<string, ReflectionTest2>();
            // Get a Type object that represents the constructed type,
            // and from that get the generic type definition. The 
            // variables d1 and d4 contain the same type.
            Type d3 = d2.GetType();
            Type d4 = d3.GetGenericTypeDefinition();

            // Display information for the generic type definition, and
            // for the constructed type Dictionary<String, Example>.
            DisplayGenericType(d1, buffer);
            DisplayGenericType(d2.GetType(), buffer);

            // Construct an array of type arguments to substitute for 
            // the type parameters of the generic Dictionary class.
            // The array must contain the correct number of types, in 
            // the same order that they appear in the type parameter 
            // list of Dictionary. The key (first type parameter)
            // is of type string, and the type to be contained in the
            // dictionary is Example.
            Type[] typeArgs = { typeof(string), typeof(ReflectionTest2) };

            // Construct the type Dictionary<String, Example>.
            Type constructed = d1.MakeGenericType(typeArgs);

            DisplayGenericType(constructed, buffer);

            object o = Activator.CreateInstance(constructed);

            buffer.AppendFormat("\r\nCompare types obtained by different methods:");
            buffer.AppendFormat("\r\n   Are the constructed types equal? {0}",
                (d2.GetType() == constructed));
            buffer.AppendFormat("\r\n   Are the generic definitions equal? {0}",
                (d1 == constructed.GetGenericTypeDefinition()));

            // Demonstrate the DisplayGenericType and 
            // DisplayGenericParameter methods with the Test class 
            // defined above. This shows base, interface, and special
            // constraints.
            DisplayGenericType(typeof(TestX<>), buffer);
        }


        // The following method displays information about a generic
        // type.
        private static void DisplayGenericType(Type t, StringBuilder buffer)
        {
            buffer.AppendFormat("\r\n {0}", t);
            buffer.AppendFormat("\r\n   Is this a generic type? {0}", t.IsGenericType);
            buffer.AppendFormat("\r\n   Is this a generic type definition? {0}", t.IsGenericTypeDefinition);

            // Get the generic type parameters or type arguments.
            Type[] typeParameters = t.GetGenericArguments();

            buffer.AppendFormat("\r\n   List {0} type arguments:", typeParameters.Length);
            foreach (Type tParam in typeParameters)
            {
                if (tParam.IsGenericParameter)
                {
                    DisplayGenericParameter(tParam);
                }
                else
                {
                    buffer.AppendFormat("\r\n      Type argument: {0}", tParam);
                }
            }
        }

        // The following method displays information about a generic
        // type parameter. Generic type parameters are represented by
        // instances of System.Type, just like ordinary types.
        private static void DisplayGenericParameter(Type tp)
        {
            StringBuilder buffer = new StringBuilder();
            buffer.AppendFormat("\r\n      Type parameter: {0} position {1}",
                tp.Name, tp.GenericParameterPosition);

            Type classConstraint = null;

            foreach (Type iConstraint in tp.GetGenericParameterConstraints())
            {
                if (iConstraint.IsInterface)
                {
                    buffer.AppendFormat("\r\n         Interface constraint: {0}",
                        iConstraint);
                }
            }

            if (classConstraint != null)
            {
                buffer.AppendFormat("\r\n         Base type constraint: {0}",
                    tp.BaseType);
            }
            else
                buffer.AppendFormat("\r\n         Base type constraint: None");

            GenericParameterAttributes sConstraints =
                tp.GenericParameterAttributes & GenericParameterAttributes.SpecialConstraintMask;

            if (sConstraints == GenericParameterAttributes.None)
            {
                buffer.AppendFormat("\r\n         No special constraints.");
            }
            else
            {
                if (GenericParameterAttributes.None != (sConstraints &
                    GenericParameterAttributes.DefaultConstructorConstraint))
                {
                    buffer.AppendFormat("\r\n         Must have a parameterless constructor.");
                }
                if (GenericParameterAttributes.None != (sConstraints &
                    GenericParameterAttributes.ReferenceTypeConstraint))
                {
                    buffer.AppendFormat("\r\n         Must be a reference type.");
                }
                if (GenericParameterAttributes.None != (sConstraints &
                    GenericParameterAttributes.NotNullableValueTypeConstraint))
                {
                    buffer.AppendFormat("\r\n         Must be a non-nullable value type.");
                }
            }
        }

    }
}
