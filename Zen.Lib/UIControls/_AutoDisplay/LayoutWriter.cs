
namespace Zen.UIControls._Misc
{
	interface IPageWriter
	{
        void WritePage();
    };

    class LayoutWriter : IPageWriter
    {
        public void WritePage()
        {
            WriteHeader();
            WriteContent();
            WriteFooter();
        }

        #region protected functions
        protected virtual bool WriteHeader()
        {
            return true;
        }

        protected virtual bool WriteContent()
        {
            return true;
        }
        protected virtual bool WriteFooter()
        {
            return true;
        }
        #endregion
    }
}
