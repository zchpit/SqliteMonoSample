using System;
using Mono.Data.Sqlite;
using Mono.CSharp;
using Simple.Data.Sqlite;
using Simple.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Dynamic;
using Microsoft.CSharp;

namespace sqlitemonosample
{
	class Program
	{
		// Holds our connection with the database
		SqliteConnection m_dbConnection;
		public static void Main (string[] args)
		{
			Program program = new Program();
		}
		public Program()
		{
			createNewDatabase();
			connectToDatabase();
			createTable();
			fillTable();
			printHighscores();
		}

		// Creates an empty database file
		void createNewDatabase()
		{
			SqliteConnection.CreateFile("MyDatabase.sqlite");
		}
		// Creates a connection with our database file.
		void connectToDatabase()
		{
			m_dbConnection = new SqliteConnection("Data Source=MyDatabase.sqlite;Version=3;");
			m_dbConnection.Open();
		}
		// Creates a table named 'highscores' with two columns: name (a string of max 20 characters) and score (an int)
		void createTable()
		{
			string sql = "create table highscores (name varchar(30), score int)";
			SqliteCommand command = new SqliteCommand(sql, m_dbConnection);
			command.ExecuteNonQuery();
		}
		// Inserts some values in the highscores table.
		// As you can see, there is quite some duplicate code here, we'll solve this in part two.
		void fillTable()
		{
			for(int i = 0; i < 10; i++)
			{
				//SimpleObj obj = new SimpleObj() { Name = i.ToString(), Score = i };
				string sql = string.Format("insert into highscores (name, score) values ('{0}', {1})", i.ToString(), i);
				SqliteCommand command = new SqliteCommand(sql, m_dbConnection);
				command.ExecuteNonQuery();
			}
		}
		// Writes the highscores to the console sorted on score in descending order.
		void printHighscores()
		{
			List<SimpleObj> dataReaderList = new List<SimpleObj>();
			Stopwatch dataReaderStopWarch = new Stopwatch();
			dataReaderStopWarch.Start();
			string sql = "select * from highscores order by score desc";
			SqliteCommand command = new SqliteCommand(sql, m_dbConnection);
			SqliteDataReader reader = command.ExecuteReader();
			while (reader.Read())
			{
				SimpleObj obj = new SimpleObj() { Name = reader["name"].ToString(), Score = Convert.ToInt32(reader["score"].ToString()) };
				dataReaderList.Add(obj);
			}
			dataReaderStopWarch.Stop();
			Console.WriteLine(string.Format("Data reader pobrał {0} obiektów w czasie {1} milisekund",dataReaderList.Count, dataReaderStopWarch.ElapsedMilliseconds));

			var connectionBuilder = new SqliteConnectionStringBuilder();
			var dir = System.IO.Directory.GetCurrentDirectory();
			connectionBuilder.DataSource = Path.Combine(dir, "MyDatabase.sqlite");
			connectionBuilder.Version = 3;

			Stopwatch simpleDataReaderStopWatch = new Stopwatch();
			List<SimpleObj> simpleDataReaderList = new List<SimpleObj>();
			simpleDataReaderStopWatch.Start();
			/*
			var cn = Database.OpenConnection(connectionBuilder.ConnectionString);
			var albums = cn.highscores.All();
			foreach (dynamic a in albums)
			{
				SimpleObj obj = new SimpleObj() { Name = a.name, Score = Convert.ToInt32(a.score) };
				simpleDataReaderList.Add(obj);
			}
			simpleDataReaderStopWatch.Stop();
			Console.WriteLine(string.Format("Simple.Data pobrał {0} obiektów w czasie {1} milisekund", simpleDataReaderList.Count, simpleDataReaderStopWatch.ElapsedMilliseconds));
*/
			Console.ReadKey();
		}
	}
}
