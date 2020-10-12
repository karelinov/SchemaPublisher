
select 
 t_diagramobjects.diagram_id,
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
((( t_diagramobjects
inner join t_object on(t_object.object_id = t_diagramobjects.object_id)) 
left outer join t_object as t_classifier1 on(t_classifier1.object_id = t_object.classifier))
left outer join t_objectproperties on (t_objectproperties.object_id=t_diagramobjects.object_id))

where
 t_diagramobjects.diagram_ID = (select diagram_ID from t_diagram where ea_Guid="#PARAM0#")

union

select 
 t_diagramobjects.diagram_id,
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
((( t_diagramobjects
inner join t_object on(t_object.object_id = t_diagramobjects.object_id)) 
left outer join t_object as t_classifier1 on(t_classifier1.object_id = t_object.classifier))
left outer join t_objectproperties on (t_objectproperties.object_id=t_classifier1.object_id))

where
 t_diagramobjects.diagram_ID = (select diagram_ID from t_diagram where ea_Guid="#PARAM0#")