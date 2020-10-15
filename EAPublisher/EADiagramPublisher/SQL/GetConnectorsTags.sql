SELECT t_connectortag.*
FROM t_connectortag
WHERE t_connectortag.ElementID IN
(
SELECT
	t_connector.Connector_ID
FROM
((t_connector as t_connector
INNER JOIN t_object as t_object_source ON t_object_source.Object_ID = t_connector.Start_Object_ID)
INNER JOIN t_object as t_object_end ON t_object_end.Object_ID = t_connector.End_Object_ID)

WHERE t_object_source.Package_ID IN (


SELECT
IIf(t_package9.Package_ID Is Null,
	IIf(t_package8.Package_ID Is Null,
		IIf(t_package7.Package_ID Is Null,
			IIf(t_package6.Package_ID Is Null,
				IIf(t_package5.Package_ID Is Null,
					IIf(t_package4.Package_ID Is Null,
						IIf(t_package3.Package_ID Is Null,
							IIf(t_package2.Package_ID Is Null,
								IIf(t_package1.Package_ID Is Null,lib_package.Package_ID,t_package1.Package_ID)
							,t_package2.Package_ID)
						,t_package3.Package_ID)
					,t_package4.Package_ID)
				,t_package5.Package_ID)
			,t_package6.Package_ID)
		,t_package7.Package_ID)
 	,t_package8.Package_ID)
,t_package9.Package_ID) AS PackageID
FROM 
(((((((((t_package lib_package 
LEFT outer JOIN t_package as t_package1 ON t_package1.Parent_ID = lib_package.Package_ID) 
LEFT outer JOIN t_package as t_package2 ON t_package2.Parent_ID = t_package1.Package_ID)
LEFT outer JOIN t_package as t_package3 ON t_package3.Parent_ID = t_package2.Package_ID)
LEFT outer JOIN t_package as t_package4 ON t_package4.Parent_ID = t_package3.Package_ID)
LEFT outer JOIN t_package as t_package5 ON t_package5.Parent_ID = t_package4.Package_ID)
LEFT outer JOIN t_package as t_package6 ON t_package6.Parent_ID = t_package5.Package_ID)
LEFT outer JOIN t_package as t_package7 ON t_package7.Parent_ID = t_package6.Package_ID)
LEFT outer JOIN t_package as t_package8 ON t_package8.Parent_ID = t_package7.Package_ID)
LEFT outer JOIN t_package as t_package9 ON t_package9.Parent_ID = t_package8.Package_ID)
WHERE lib_package.ea_guid="#PARAM0#" 

)

)