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
WHERE t_object.Package_ID IN (#PARAM0#)

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
WHERE t_object.Package_ID IN (#PARAM0#)

