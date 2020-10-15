select 
 t_object.package_id,
 t_object.name,
 t_object.object_type,
 t_object.object_id,
 t_object.classifier_guid,
 t_object.note,
 t_classifier1.object_id as classifier_id,
 t_classifier1.name as classifier_name,
 t_classifier1.object_type as classifier_type,
 t_objectproperties.property,
 t_objectproperties.value
FROM 
((t_object 
left outer join t_object as t_classifier1 on(t_classifier1.object_id = t_object.classifier))
left outer join t_objectproperties on (t_objectproperties.object_id=t_object.object_id))

WHERE t_object.Package_ID IN (
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

union

select 
 t_object.package_id,
 t_object.name,
 t_object.object_type,
 t_object.object_id,
 t_object.classifier_guid,
 t_object.note,
 t_classifier1.object_id as classifier_id,
 t_classifier1.name as classifier_name,
 t_classifier1.object_type as classifier_type,
 t_objectproperties.property,
 t_objectproperties.value
FROM 
((t_object 
left outer join t_object as t_classifier1 on(t_classifier1.object_id = t_object.classifier))
left outer join t_objectproperties on (t_objectproperties.object_id=t_classifier1.object_id))

WHERE t_object.Package_ID IN (
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