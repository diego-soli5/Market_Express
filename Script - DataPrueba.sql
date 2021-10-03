
INSERT INTO Permission(Id,Name) VALUES
('03B93003-B8F0-4315-A0C0-449E5058F23A','Permiso 1'),
('1AAF51B4-B054-4487-9FC5-26B96886E737','Permiso 2'),
('C236C5C0-004C-4B1C-A3E7-0E427E9F9593','Permiso 3'),
('0C13E8A6-3A0A-419E-9D18-778D8DFC87D6','Permiso 4'),
('9E66A172-D4C0-4FFB-B2F8-59471A826C17','Permiso 5');
GO

INSERT INTO Role(Id,Name,CreationDate,AddedBy) VALUES
('EC81F591-CA24-4A80-8D54-316897E9015C','Todos los permisos',GETDATE(),'SYSTEM'),
('DF22A0FF-6FD5-4422-977D-49B99D3D71C2','Permiso 1 y 2',GETDATE(),'SYSTEM'),
('C4934D01-205D-4888-827C-8787AB6B3CEE','Permiso 4 y 5',GETDATE(),'SYSTEM');
GO

INSERT INTO Role_Permission(RoleId,PermissionId) VALUES
('EC81F591-CA24-4A80-8D54-316897E9015C','03B93003-B8F0-4315-A0C0-449E5058F23A'), --Todos los permisos
('EC81F591-CA24-4A80-8D54-316897E9015C','1AAF51B4-B054-4487-9FC5-26B96886E737'), --Todos los permisos
('EC81F591-CA24-4A80-8D54-316897E9015C','C236C5C0-004C-4B1C-A3E7-0E427E9F9593'), --Todos los permisos
('EC81F591-CA24-4A80-8D54-316897E9015C','0C13E8A6-3A0A-419E-9D18-778D8DFC87D6'), --Todos los permisos
('EC81F591-CA24-4A80-8D54-316897E9015C','9E66A172-D4C0-4FFB-B2F8-59471A826C17'), --Todos los permisos
('DF22A0FF-6FD5-4422-977D-49B99D3D71C2','03B93003-B8F0-4315-A0C0-449E5058F23A'), --Permiso 1 y 2
('DF22A0FF-6FD5-4422-977D-49B99D3D71C2','1AAF51B4-B054-4487-9FC5-26B96886E737'), --Permiso 1 y 2
('C4934D01-205D-4888-827C-8787AB6B3CEE','0C13E8A6-3A0A-419E-9D18-778D8DFC87D6'), --Permiso 4 y 5
('C4934D01-205D-4888-827C-8787AB6B3CEE','9E66A172-D4C0-4FFB-B2F8-59471A826C17'); --Permiso 4 y 5
GO

INSERT INTO AppUser(Id,Name,Alias,Identification,Email,Phone,Password,Type,Status,CreationDate,AddedBy) VALUES
('EA16E721-5E1D-EC11-9953-3863BBBB3AE0','Luis Diego Sol�s Camacho','Diego','1-1731-0010','1diego321@gmail.com','83358092','10000.+UfMddrk8Z1k7UZBQDNPvA==.705k1c4kJPT9uYc77Fkjw2/VAl257UUmJkSj0jGY/Zo=','ADMINISTRADOR','ACTIVADO',GETDATE(),'SYSTEM');
GO

INSERT INTO AppUser_Role(RoleId,AppUserId) VALUES
('EC81F591-CA24-4A80-8D54-316897E9015C','EA16E721-5E1D-EC11-9953-3863BBBB3AE0') --Todos los permisos
--('DF22A0FF-6FD5-4422-977D-49B99D3D71C2','EA16E721-5E1D-EC11-9953-3863BBBB3AE0'), --Permiso 1 y 2
--('C4934D01-205D-4888-827C-8787AB6B3CEE','EA16E721-5E1D-EC11-9953-3863BBBB3AE0') --Permiso 4 y 5
GO

INSERT INTO Client(Id,AppUserId,AutoSync) VALUES
('9229F064-0922-EC11-9955-3863BBBB3AE0','EA16E721-5E1D-EC11-9953-3863BBBB3AE0',0);  
GO

INSERT INTO Cart(Id,ClientId,OpeningDate,Status) VALUES
('321F7AFB-3C22-EC11-9955-3863BBBB3AE0','9229F064-0922-EC11-9955-3863BBBB3AE0',GETDATE(),'ABIERTO')

INSERT INTO Cart_Detail(CartId,ArticleId,Quantity) VALUES
('321F7AFB-3C22-EC11-9955-3863BBBB3AE0','C812E64E-6F1E-42D5-A47B-0084B6EF265A',1),
('321F7AFB-3C22-EC11-9955-3863BBBB3AE0','7E851779-83C5-48FD-8A3E-00DA91D9DF3C',3);
