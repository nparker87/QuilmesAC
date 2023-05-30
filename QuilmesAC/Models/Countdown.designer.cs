﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuilmesAC.Models
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="Countdown")]
	public partial class CountdownDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertEvent(Event instance);
    partial void UpdateEvent(Event instance);
    partial void DeleteEvent(Event instance);
    #endregion
		
		public CountdownDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["CountdownConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public CountdownDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public CountdownDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public CountdownDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public CountdownDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Event> Events
		{
			get
			{
				return this.GetTable<Event>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Event")]
	public partial class Event : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _ID;
		
		private string _Name;
		
		private string _Description;
		
		private System.Nullable<System.DateTime> _StartDate;
		
		private System.Nullable<System.DateTime> _EndDate;
		
		private string _ThemeImage;
		
		private System.DateTime _DateAdded;
		
		private System.DateTime _DateChanged;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(long value);
    partial void OnIDChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnDescriptionChanging(string value);
    partial void OnDescriptionChanged();
    partial void OnStartDateChanging(System.Nullable<System.DateTime> value);
    partial void OnStartDateChanged();
    partial void OnEndDateChanging(System.Nullable<System.DateTime> value);
    partial void OnEndDateChanged();
    partial void OnThemeImageChanging(string value);
    partial void OnThemeImageChanged();
    partial void OnDateAddedChanging(System.DateTime value);
    partial void OnDateAddedChanged();
    partial void OnDateChangedChanging(System.DateTime value);
    partial void OnDateChangedChanged();
    #endregion
		
		public Event()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(255) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Description", DbType="NVarChar(MAX)")]
		public string Description
		{
			get
			{
				return this._Description;
			}
			set
			{
				if ((this._Description != value))
				{
					this.OnDescriptionChanging(value);
					this.SendPropertyChanging();
					this._Description = value;
					this.SendPropertyChanged("Description");
					this.OnDescriptionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_StartDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> StartDate
		{
			get
			{
				return this._StartDate;
			}
			set
			{
				if ((this._StartDate != value))
				{
					this.OnStartDateChanging(value);
					this.SendPropertyChanging();
					this._StartDate = value;
					this.SendPropertyChanged("StartDate");
					this.OnStartDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EndDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> EndDate
		{
			get
			{
				return this._EndDate;
			}
			set
			{
				if ((this._EndDate != value))
				{
					this.OnEndDateChanging(value);
					this.SendPropertyChanging();
					this._EndDate = value;
					this.SendPropertyChanged("EndDate");
					this.OnEndDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ThemeImage", DbType="NVarChar(255)")]
		public string ThemeImage
		{
			get
			{
				return this._ThemeImage;
			}
			set
			{
				if ((this._ThemeImage != value))
				{
					this.OnThemeImageChanging(value);
					this.SendPropertyChanging();
					this._ThemeImage = value;
					this.SendPropertyChanged("ThemeImage");
					this.OnThemeImageChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DateAdded", DbType="DateTime NOT NULL")]
		public System.DateTime DateAdded
		{
			get
			{
				return this._DateAdded;
			}
			set
			{
				if ((this._DateAdded != value))
				{
					this.OnDateAddedChanging(value);
					this.SendPropertyChanging();
					this._DateAdded = value;
					this.SendPropertyChanged("DateAdded");
					this.OnDateAddedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DateChanged", DbType="DateTime NOT NULL")]
		public System.DateTime DateChanged
		{
			get
			{
				return this._DateChanged;
			}
			set
			{
				if ((this._DateChanged != value))
				{
					this.OnDateChangedChanging(value);
					this.SendPropertyChanging();
					this._DateChanged = value;
					this.SendPropertyChanged("DateChanged");
					this.OnDateChangedChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591