namespace _TestProject
{
    using TestContext = Microsoft.VisualStudio.TestTools.UnitTesting.TestContext;

    public abstract class TestBase
    {
        public TestContext TestContext
        {
            get { return _testContextInstance; }
            set { _testContextInstance = value; }
        }

        #region private data
        private TestContext _testContextInstance;
        #endregion
    }
}
