CREATE VIEW [SLTDS_C1_E-tableView]
AS 
SELECT ET.*, 
   CF.[HasOCR], CF.[HasImage], CF.[Creator_UserID], CF.[Modified_userID], CF.[EDocTypeID], CF.[EDocID], CF.[ProdNumber], CF.[ProdTitle], T1_SP.*, TD.[TEXT], EF.[EDocText] 
FROM dbo.[SLTDS_C1_E-table] ET 
  LEFT JOIN dbo.[SLT_C1_CommonFields] CF ON CF.RowGUID = ET.RowGUID 
  LEFT JOIN [dbo].[SLT_SecurityPrincipals] T1_SP ON T1_SP.PrincipalID = CF.[Modified_UserID] 
  LEFT JOIN [dbo].[SLT_C1_TextDocuments] TD ON TD.PrimaryRowGUID = ET.RowGUID 
  LEFT JOIN [dbo].[SLT_C1_EDocFiles] EF ON EF.EDocID = CF.EDocID 
GO

CREATE VIEW SLTDS_C1_ETableView_GS 
AS 
SELECT ET.*, 
	CF.[HasOCR],
	CF.[HasImage],
	CF.[Creator_UserID],
	CF.[Modified_userID],
	CF.[EDocTypeID],
	CF.[ProdNumber] CFProdNumber,
	CF.[ProdTitle],
	ED.EDocID, ED.HashValue, ED.FileStartBytes, ED.OrigFileName, ED.FileSize as fs, ED.EDocText,
	ED.DateCreated EDD, ED.Creator_UserID EDCU, ED.DateModified EDM, ED.Modifier_UserID EDMU,
	Production.ProductionSetID PSID, Production.ProdNumber, Production.ProductionNumber, Production.DocumentProductionTypeID
FROM [SLTDS_C1_E-table] ET
INNER JOIN [SLT_C1_CommonFields] CF ON ET.rowguid = CF.RowGUID
LEFT JOIN [SLT_C1_TextDocuments] TD ON TD.PrimaryROWGUID = CF.RowGUID
LEFT JOIN [SLT_C1_EDocFiles] ED ON ED.EDocID = CF.EDocID
LEFT JOIN (select 
				PNR.PrimaryRowGUID, PNR.ProductionSetID PSID, PNR.ProdNumber, PSM.ProductionNumber, PSM.DocumentProductionTypeID
			,PS.*
			from 
			[SLT_C1_ProdNUmberRange] PNR,
			[SLT_C1_ProductionSetMembers] PSM
			,[SLT_C1_ProductionSets] PS
			WHERE
			PNR.PrimaryRowGUID = PSM.rowGUID
			AND PNR.ProductionSetId = PSM.ProductionSetID
			AND PS.ProductionSetId = PSM.productionSetID) Production
ON ET.RowGUID = Production.PrimaryRowGUID
GO

CREATE VIEW [dbo].[SLTDS_C1_ETableView_GS_Brief] 
AS 
SELECT ET.*, 
	CF.[HasOCR],
	CF.[HasImage],
	CF.[Creator_UserID],
	CF.[Modified_userID],
	CF.[EDocTypeID],
	CF.[ProdNumber] CFProdNumber,
	CF.[ProdTitle],
	ED.EDocID, ED.HashValue, ED.FileStartBytes, ED.OrigFileName, ED.FileSize as fs, ED.EDocText,
	ED.DateCreated EDD, ED.Creator_UserID EDCU, ED.DateModified EDM, ED.Modifier_UserID EDMU
FROM [SLTDS_C1_E-table] ET
INNER JOIN [SLT_C1_CommonFields] CF ON ET.rowguid = CF.RowGUID
LEFT JOIN [SLT_C1_TextDocuments] TD ON TD.PrimaryROWGUID = CF.RowGUID
LEFT JOIN [SLT_C1_EDocFiles] ED ON ED.EDocID = CF.EDocID
GO

CREATE TABLE [CTS_IDX_DELETED_RECORDS]
(
    [CaseName] [nvarchar](70) NOT NULL,
    [TableName] [nvarchar](70) NOT NULL,
    [DeletedRecords] [nvarchar] (max) NULL
)

/* Alter table schema and add Indexed column*/
Declare @sql nvarchar(max)
set @sql = ''

SELECT @sql = @sql + ' ALTER TABLE [' + table_name + '] ADD Indexed INT'
 FROM information_schema.tables 
WHERE (table_name like 'SLTME%' or table_name like 'SLTDS%'
               or table_name like 'SLT%') 
  and table_name not like 'slt_casemodelfie%' 
Execute sp_executesql @sql
 
