// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Code Analysis results, point to "Suppress Message", and click 
// "In Suppression File".
// You do not need to add suppressions to this file manually.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Scope = "member", Target = "Phytel.Web.MongoSessionStateStore.#Serialize(System.Web.SessionState.SessionStateItemCollection)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Scope = "member", Target = "Phytel.Web.MongoSessionStateStore.#Deserialize(System.Web.HttpContext,System.String,System.Int32)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Scope = "type", Target = "Phytel.Web.MongoSessionStateDatabaseWrapper")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Scope = "member", Target = "Phytel.Web.MongoSessionStateDatabaseWrapper.#Dispose()")]
