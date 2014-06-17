// ***********************************************************************
// Assembly         : WpfApplication1
// Author           : user
// Created          : 06-10-2014
//
// Last Modified By : user
// Last Modified On : 06-10-2014
// ***********************************************************************
// <copyright file="DBConnection.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;

namespace project
{
    /// <summary>
    /// Class DBConnection.
    /// </summary>
    class DBConnection
    {
        //static string host = "localhost";
        //static string server = "SQLEXPRESS";
        /// <summary>
        /// The adapter
        /// </summary>
        public MySqlDataAdapter adapter;
        /// <summary>
        /// The dataset
        /// </summary>
        public DataSet dataset;
        /// <summary>
        /// The object connection
        /// </summary>
        public MySqlConnection objConnection;
        /// <summary>
        /// The object command
        /// </summary>
        public MySqlCommand objCommand;

        /// <summary>
        /// Gets the data table from database.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>DataSet.</returns>
        public DataSet GetDataTableFromDB(string query)
        {
            string Connectionstring = "Data Source=localhost; Username=root;Password=1234; Initial Catalog=project";
            this.objConnection = new MySqlConnection(Connectionstring);
            try
            {
                this.objConnection.Open();
                this.objCommand = new MySqlCommand(query, this.objConnection);
                this.dataset = new DataSet();
                this.adapter = new MySqlDataAdapter(this.objCommand);
                this.adapter.Fill(this.dataset);
                try
                {
                    return this.dataset;
                }
                catch (Exception e)
                {
                    MessageBox.Show("התחברות למסד הנתונים כשלה!," + e.Message);
                    return null;
                }
            }
            finally
            {
                this.objConnection.Close();
            }
        }

        /// <summary>
        /// Inserts the data into database.
        /// </summary>
        /// <param name="Connectionstring">The connectionstring.</param>
        /// <param name="query">The query.</param>
        /// <returns>DataSet.</returns>
        public DataSet InsertDataIntoDB(string Connectionstring, string query)
        {
            this.objConnection = new MySqlConnection(Connectionstring);
            try
            {
                this.objConnection.Open();
                this.objCommand = new MySqlCommand(query, this.objConnection);
                this.dataset = new DataSet();
              //  objCommand.ExecuteNonQuery();
                this.adapter = new MySqlDataAdapter(this.objCommand);
             //   Console.WriteLine("שאילתת ההוספה");
             //   Console.WriteLine(query);
                this.adapter.Fill(this.dataset);
             //   Console.WriteLine("הדטה סט המתקבל");
            //    Console.WriteLine(this.dataset);
                try
                {
                    MessageBox.Show("המידע נשמר בהצלחה!", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                    return this.dataset;
                }
                catch (Exception e)
                {
                    MessageBox.Show("התחברות למסד הנתונים כשלה!," + e.Message);
                    return null;
                }
            }
            finally
            {
                this.objConnection.Close();
            }
        }

        /// <summary>
        /// Deletes the data from database.
        /// </summary>
        /// <param name="query">The query.</param>
        public void DeleteDataFromDB(string query)
        {
            string Connectionstring = "Data Source=localhost; Username=root;Password=1234; Initial Catalog=project"; ;
            MySqlConnection objConnection = new MySqlConnection(Connectionstring);
            try
            {
                objConnection.Open();
               MySqlCommand objCommand = new MySqlCommand(query, objConnection);
                objCommand.ExecuteNonQuery();
                //MessageBox.Show("המידע נמחק בהצלחה!");
                objConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("התחברות למסד הנתונים כשלה!," + e.Message);

            }
        }

        /// <summary>
        /// Updates the data into database.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>DataSet.</returns>
        public DataSet UpdateDataIntoDB(string query)
        {
            string Connectionstring = "Data Source=localhost; Username=root;Password=1234; Initial Catalog=project";
            this.objConnection = new MySqlConnection(Connectionstring);
            try
            {
                this.objConnection.Open();
                this.objCommand = new MySqlCommand(query, this.objConnection);
                this.dataset = new DataSet();
                this.adapter = new MySqlDataAdapter(this.objCommand);
                this.adapter.Fill(this.dataset);
                try
                {
                    MessageBox.Show("המידע עודכן בהצלחה!");
                    return this.dataset;
                }
                catch (Exception e)
                {
                    MessageBox.Show("התחברות למסד הנתונים כשלה!," + e.Message);
                    return null;
                }
            }
            finally
            {
                this.objConnection.Close();
            }
        }

        /// <summary>
        /// Logs the user in.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="Connectionstring">The connectionstring.</param>
        /// <returns>DataSet.</returns>
        public DataSet LogIn(string query, string Connectionstring)
        {
            //string Connectionstring = " SERVER=localhost;DATABASE=project; UId=root;Password=1234;";
            
            this.objConnection = new MySqlConnection(Connectionstring);
            try
            {
                this.objConnection.Open();
                this.objCommand = new MySqlCommand(query, this.objConnection);
                this.dataset = new DataSet();
                this.adapter = new MySqlDataAdapter(this.objCommand);
                this.adapter.Fill(this.dataset);
                try
                {
                    return this.dataset;
                }
                catch (Exception e)
                {
                    MessageBox.Show("התחברות למסד הנתונים כשלה!," + e.Message);
                    return null;
                }
            }
            finally
            {
                this.objConnection.Close();
            }
        }

        /// <summary>
        /// Logs the user the out.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>DataSet.</returns>
        public DataSet LogOut(string query)
        {

            string Connectionstring = "Server=localhost; Database=project;Uid=root; pwd=1234";
            this.objConnection = new MySqlConnection(Connectionstring);
            try
            {
                this.objConnection.Open();
                this.objCommand = new MySqlCommand(query, this.objConnection);
                this.dataset = new DataSet();
                this.adapter = new MySqlDataAdapter(this.objCommand);
                this.adapter.Fill(this.dataset);
                try
                {
                    return this.dataset;
                }
                catch (Exception e)
                {
                    MessageBox.Show("התחברות למסד הנתונים כשלה!," + e.Message);
                    return null;
                }
            }
            finally
            {
                this.objConnection.Close();
            }
        }
    }
}