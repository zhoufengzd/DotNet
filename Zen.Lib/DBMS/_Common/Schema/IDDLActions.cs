using System;
using System.Collections.Generic;
using System.Text;

namespace Zen.DBMS.Schema
{
    public enum DDLActionEnum
    {
        NONE = 0,
        CREATE = 1,
        DROP,
        MODIFY,
    }

    #region IDDLBuilder interface

    // IDDLBuilder: interface that supports Create / Drop / Alter schema object
    public interface IDDLCreator
    {
        string BuildCreateDDL();
    }
    public interface IDDLDroper
    {
        string BuildDropDDL();
    }
    public interface IDDLAlterer
    {
        string BuildAlterDDL(int nAction);	// nAction could be casted to a customized enum type
    }
    public interface IDDLBuilder : IDDLCreator, IDDLDroper, IDDLAlterer
    {
    }
    #endregion
}
