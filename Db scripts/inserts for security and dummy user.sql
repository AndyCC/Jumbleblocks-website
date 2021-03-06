  use [BlogDb]
  
  SET IDENTITY_INSERT Operations ON
  INSERT INTO Operations(Id, Name) VALUES (1, 'Create Blog Post')
  INSERT INTO Operations(Id, Name) VALUES (2, 'Delete Blog Post')
  INSERT INTO Operations(Id, Name) VALUES (3, 'Edit Blog Post')
  SET IDENTITY_INSERT Operations OFF
  
    
  SET IDENTITY_INSERT Roles ON
  INSERT INTO Roles(Id, Name) VALUES (1, 'Blog Author')
  SET IDENTITY_INSERT Roles OFF
  
  INSERT INTO RoleOperations(RoleId, OperationId) VALUES(1,1)
  INSERT INTO RoleOperations(RoleId, OperationId) VALUES(1,2)
  INSERT INTO RoleOperations(RoleId, OperationId) VALUES(1,3)
  
  SET IDENTITY_INSERT Users ON
  INSERT INTO Users(Id, Forenames, Surname, Username, Password) VALUES (1, 'Dummy', 'User', 'DummyUser', '2683f6f800772137f0a1e839f4ff20e4') --password is DummyUser
  SET IDENTITY_INSERT Users OFF
  
  INSERT INTO UserRoles(UserId, RoleId) VALUES (1,1)