Getting Started

At first, you have to provide the connection string in your we.config/app.config file as so:

  <connectionStrings>
    <add name="Membership" connectionString="server=192.168.1.1;database=Test;user id=kamyar;password=secret;" providerName="System.Data.SqlClient" />
  </connectionStrings>

Do not forget to add the 'ProviderName', for that the library can work with any of them!  Not limited to MS SQL Server. You can use MySQl, my favorite
PostGresql, Oracle, SQlite or any other database supporting Stored Procedures.

Afterwards, you must create an interface containing the stored procedure signatures:

public interface IMembershipDatabase
{
	int GetUsersCount();
	bool CreateUser(string username, string password);
	IEnumerable<User> GetUserList();
	void GetUserBalance(string username, out decimal balance, out decimal discount);
}

As you can see you can even use 'out' parameters and IEnumerable for stored procedures returning tables. You dant want to define POCO classes? no problem, This 
library creates one on-the-fly for you, just replace the Class name with object keyword:

	IEnumerable<object> GetUserList();
	
This makes you confortable when for example serializing the class as JSON.

The last part is to write system wirings: for example, if you are making a web project, you can write these two lines in Global.asax file:

	Setup.Register<IMembershipDatabase>("Membership");
	
Ready! Now we can use the soterd procedures like any normal C# method:

	var db = Setup.GetInstance<IMembershipDatabase>();
	var count = db.getUserCount();
	
	decimal balance, discount;
	db.GetUserBalance("John", out balance, out discount);
	
Good luck!

(Release 1.3.0 change: No need to call Setup.Initialize. Method removed)

Release 1.3.6 change: You can use your favorite parameter name, using DbName attribute:

	bool CreateUser([DBName("username") string uname, string password);
