



ALTER TABLE Employees
ADD Role VARCHAR(50);

select * from Employees

UPDATE Employees
SET Role = 'staff'

UPDATE Employees
SET Role = 'management'
WHERE Employees_ID IN (1,2,3)

ALTER TABLE Employees
ADD CONSTRAINT DF_Employees_Role DEFAULT 'staff' FOR Role;




ALTER TABLE Employees
ADD Password NVARCHAR(6)

UPDATE Employees
SET Password = LEFT(CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR(10)), 6);



UPDATE Employees
SET Phone = REPLACE(Phone, '-' , '')


UPDATE Employees
SET Phone = REPLACE(Phone, ' ' , '')