SELECT
lib_package.package_id
FROM t_package lib_package
WHERE lib_package.ea_guid="#PARAM0#"
UNION 
SELECT
t_package1.Package_ID AS package_id
FROM (t_package lib_package
INNER JOIN t_package as t_package1 ON (t_package1.Parent_ID = lib_package.Package_ID))
WHERE lib_package.ea_guid="#PARAM0#"
UNION 
SELECT
t_package2.Package_ID AS package_id
FROM ((t_package lib_package
INNER JOIN t_package as t_package1 ON (t_package1.Parent_ID = lib_package.Package_ID))
INNER JOIN t_package as t_package2 ON (t_package2.Parent_ID = t_package1.Package_ID))
WHERE lib_package.ea_guid="#PARAM0#"
UNION 
SELECT
t_package3.Package_ID AS package_id
FROM (((t_package lib_package
INNER JOIN t_package as t_package1 ON (t_package1.Parent_ID = lib_package.Package_ID))
INNER JOIN t_package as t_package2 ON (t_package2.Parent_ID = t_package1.Package_ID))
INNER JOIN t_package as t_package3 ON (t_package3.Parent_ID = t_package2.Package_ID))
WHERE lib_package.ea_guid="#PARAM0#"
UNION 
SELECT
t_package4.Package_ID AS package_id
FROM ((((t_package lib_package
INNER JOIN t_package as t_package1 ON (t_package1.Parent_ID = lib_package.Package_ID))
INNER JOIN t_package as t_package2 ON (t_package2.Parent_ID = t_package1.Package_ID))
INNER JOIN t_package as t_package3 ON (t_package3.Parent_ID = t_package2.Package_ID))
INNER JOIN t_package as t_package4 ON (t_package4.Parent_ID = t_package3.Package_ID))
WHERE lib_package.ea_guid="#PARAM0#"
UNION 
SELECT
t_package5.Package_ID AS package_id
FROM (((((t_package lib_package
INNER JOIN t_package as t_package1 ON (t_package1.Parent_ID = lib_package.Package_ID))
INNER JOIN t_package as t_package2 ON (t_package2.Parent_ID = t_package1.Package_ID))
INNER JOIN t_package as t_package3 ON (t_package3.Parent_ID = t_package2.Package_ID))
INNER JOIN t_package as t_package4 ON (t_package4.Parent_ID = t_package3.Package_ID))
INNER JOIN t_package as t_package5 ON (t_package5.Parent_ID = t_package4.Package_ID))
WHERE lib_package.ea_guid="#PARAM0#"
UNION 
SELECT
t_package6.Package_ID AS package_id
FROM ((((((t_package lib_package
INNER JOIN t_package as t_package1 ON (t_package1.Parent_ID = lib_package.Package_ID))
INNER JOIN t_package as t_package2 ON (t_package2.Parent_ID = t_package1.Package_ID))
INNER JOIN t_package as t_package3 ON (t_package3.Parent_ID = t_package2.Package_ID))
INNER JOIN t_package as t_package4 ON (t_package4.Parent_ID = t_package3.Package_ID))
INNER JOIN t_package as t_package5 ON (t_package5.Parent_ID = t_package4.Package_ID))
INNER JOIN t_package as t_package6 ON (t_package6.Parent_ID = t_package5.Package_ID))
WHERE lib_package.ea_guid="#PARAM0#"
UNION 
SELECT
t_package7.Package_ID AS package_id
FROM (((((((t_package lib_package
INNER JOIN t_package as t_package1 ON (t_package1.Parent_ID = lib_package.Package_ID))
INNER JOIN t_package as t_package2 ON (t_package2.Parent_ID = t_package1.Package_ID))
INNER JOIN t_package as t_package3 ON (t_package3.Parent_ID = t_package2.Package_ID))
INNER JOIN t_package as t_package4 ON (t_package4.Parent_ID = t_package3.Package_ID))
INNER JOIN t_package as t_package5 ON (t_package5.Parent_ID = t_package4.Package_ID))
INNER JOIN t_package as t_package6 ON (t_package6.Parent_ID = t_package5.Package_ID))
INNER JOIN t_package as t_package7 ON (t_package7.Parent_ID = t_package6.Package_ID))
WHERE lib_package.ea_guid="#PARAM0#"
UNION 
SELECT
t_package8.Package_ID AS package_id
FROM ((((((((t_package lib_package
INNER JOIN t_package as t_package1 ON (t_package1.Parent_ID = lib_package.Package_ID))
INNER JOIN t_package as t_package2 ON (t_package2.Parent_ID = t_package1.Package_ID))
INNER JOIN t_package as t_package3 ON (t_package3.Parent_ID = t_package2.Package_ID))
INNER JOIN t_package as t_package4 ON (t_package4.Parent_ID = t_package3.Package_ID))
INNER JOIN t_package as t_package5 ON (t_package5.Parent_ID = t_package4.Package_ID))
INNER JOIN t_package as t_package6 ON (t_package6.Parent_ID = t_package5.Package_ID))
INNER JOIN t_package as t_package7 ON (t_package7.Parent_ID = t_package6.Package_ID))
INNER JOIN t_package as t_package8 ON (t_package8.Parent_ID = t_package7.Package_ID))
WHERE lib_package.ea_guid="#PARAM0#"



