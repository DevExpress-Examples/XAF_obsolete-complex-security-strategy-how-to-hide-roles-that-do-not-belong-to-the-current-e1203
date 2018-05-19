Imports Microsoft.VisualBasic
Imports System

Imports DevExpress.ExpressApp.Updating
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.ExpressApp.Security

Namespace DXExample.Module
	Public Class Updater
		Inherits ModuleUpdater
		Public Sub New(ByVal session As Session, ByVal currentDBVersion As Version)
			MyBase.New(session, currentDBVersion)
		End Sub
		Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
			MyBase.UpdateDatabaseAfterUpdateSchema()
			' If a user named 'Sam' doesn't exist in the database, create this user
			Dim user1 As CustomUser = Session.FindObject(Of CustomUser)(New BinaryOperator("UserName", "Sam"))
			If user1 Is Nothing Then
				user1 = New CustomUser(Session)
				user1.UserName = "Sam"
				user1.FirstName = "Sam"
				' Set a password if the standard authentication type is used
				user1.SetPassword("")
			End If
			' If a user named 'John' doesn't exist in the database, create this user
			Dim user2 As CustomUser = Session.FindObject(Of CustomUser)(New BinaryOperator("UserName", "John"))
			If user2 Is Nothing Then
				user2 = New CustomUser(Session)
				user2.UserName = "John"
				user2.FirstName = "John"
				' Set a password if the standard authentication type is used
				user2.SetPassword("")
			End If
			' If a user named 'John' doesn't exist in the database, create this user
			Dim user3 As CustomUser = Session.FindObject(Of CustomUser)(New BinaryOperator("UserName", "Mary"))
			If user3 Is Nothing Then
				user3 = New CustomUser(Session)
				user3.UserName = "Mary"
				user3.FirstName = "Mary"
				' Set a password if the standard authentication type is used
				user3.SetPassword("")
			End If
			' If a role with the Administrators name doesn't exist in the database, create this role
			Dim adminRole As CustomRole = Session.FindObject(Of CustomRole)(New BinaryOperator("Name", "Administrators"))
			If adminRole Is Nothing Then
				adminRole = New CustomRole(Session)
				adminRole.Name = "Administrators"
			End If
			' If a role with the Users name doesn't exist in the database, create this role
			Dim userRole As CustomRole = Session.FindObject(Of CustomRole)(New BinaryOperator("Name", "Users"))
			If userRole Is Nothing Then
				userRole = New CustomRole(Session)
				userRole.Name = "Users"
			End If
			' If a role with the PowerUsers name doesn't exist in the database, create this role
			Dim powerUserRole As CustomRole = Session.FindObject(Of CustomRole)(New BinaryOperator("Name", "PowerUsers"))
			If powerUserRole Is Nothing Then
				powerUserRole = New CustomRole(Session)
				powerUserRole.Name = "PowerUsers"
			End If
			' Delete all permissions assigned to the Administrators and Users roles
			Do While adminRole.PersistentPermissions.Count > 0
				Session.Delete(adminRole.PersistentPermissions(0))
			Loop
			Do While userRole.PersistentPermissions.Count > 0
				Session.Delete(userRole.PersistentPermissions(0))
			Loop
			Do While powerUserRole.PersistentPermissions.Count > 0
				Session.Delete(powerUserRole.PersistentPermissions(0))
			Loop
			' Allow full access to all objects to the Administrators role
			adminRole.AddPermission(New ObjectAccessPermission(GetType(Object), ObjectAccess.AllAccess))
			' Allow editing the Application Model to the Administrators role
			adminRole.AddPermission(New EditModelPermission(ModelAccessModifier.Allow))
			adminRole.IsOwningRequired = True
			' Save the Administrators role to the database
			adminRole.Save()
			' Allow full access to all objects to the Users role
			userRole.AddPermission(New ObjectAccessPermission(GetType(Object), ObjectAccess.AllAccess))
			' Deny change access to the User type objects to the Users role
			userRole.AddPermission(New ObjectAccessPermission(GetType(User), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny))
			' Deny full access to the Role type objects to the Users role
			userRole.AddPermission(New ObjectAccessPermission(GetType(Role), ObjectAccess.AllAccess, ObjectAccessModifier.Deny))
			' Deny editing the Application Model to the Users role
			userRole.AddPermission(New EditModelPermission(ModelAccessModifier.Deny))
			' Save the Users role to the database
			userRole.Save()
			' Allow full access to all objects to the Administrators role
			powerUserRole.AddPermission(New ObjectAccessPermission(GetType(Object), ObjectAccess.AllAccess))
			' Allow editing the Application Model to the Administrators role
            powerUserRole.AddPermission(New EditModelPermission(ModelAccessModifier.Deny))
            ' Deny creating the Role type objects to the Users role
            userRole.AddPermission(New ObjectAccessPermission(GetType(Role), ObjectAccess.Create, ObjectAccessModifier.Deny))
			powerUserRole.IsOwningRequired = True
			' Save the Administrators role to the database
			powerUserRole.Save()
			' Add the Administrators role to the user1
			user1.Roles.Add(adminRole)
			' Add the Users role to the user2
			user2.Roles.Add(userRole)
			' Add the PowerUsers role to the user3
			user3.Roles.Add(powerUserRole)
			' Save the users to the database
			user1.Save()
			user2.Save()
			user3.Save()
		End Sub
	End Class
End Namespace
