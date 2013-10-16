﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the ContextGenerator.ttinclude code generation file.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Common;
using System.Collections.Generic;
using Telerik.OpenAccess;
using Telerik.OpenAccess.Metadata;
using Telerik.OpenAccess.Data.Common;
using Telerik.OpenAccess.Metadata.Fluent;
using Telerik.OpenAccess.Metadata.Fluent.Advanced;
using WebPortal;

namespace WebPortal	
{
	public partial class EntitiesModel : OpenAccessContext, IEntitiesModelUnitOfWork
	{
		private static string connectionStringName = @"TraDataConnection1";
			
		private static BackendConfiguration backend = GetBackendConfiguration();
				
		private static MetadataSource metadataSource = XmlMetadataSource.FromAssemblyResource("EntitiesModel.rlinq");
		
		public EntitiesModel()
			:base(connectionStringName, backend, metadataSource)
		{ }
		
		public EntitiesModel(string connection)
			:base(connection, backend, metadataSource)
		{ }
		
		public EntitiesModel(BackendConfiguration backendConfiguration)
			:base(connectionStringName, backendConfiguration, metadataSource)
		{ }
			
		public EntitiesModel(string connection, MetadataSource metadataSource)
			:base(connection, backend, metadataSource)
		{ }
		
		public EntitiesModel(string connection, BackendConfiguration backendConfiguration, MetadataSource metadataSource)
			:base(connection, backendConfiguration, metadataSource)
		{ }
			
		public IQueryable<Scanned> Scanneds 
		{
			get
			{
				return this.GetAll<Scanned>();
			}
		}
		
		public IQueryable<ScanGroup> ScanGroups 
		{
			get
			{
				return this.GetAll<ScanGroup>();
			}
		}
		
		public IQueryable<DAT9397F> DAT9397Fs 
		{
			get
			{
				return this.GetAll<DAT9397F>();
			}
		}
		
		public IQueryable<DAT8000> DAT8000 
		{
			get
			{
				return this.GetAll<DAT8000>();
			}
		}
		
		public IQueryable<DAT2000> DAT2000 
		{
			get
			{
				return this.GetAll<DAT2000>();
			}
		}
		
		public IQueryable<BookTable> BookTables 
		{
			get
			{
				return this.GetAll<BookTable>();
			}
		}
		
		public IQueryable<Apoint> Apoints 
		{
			get
			{
				return this.GetAll<Apoint>();
			}
		}
		
		public IQueryable<DAT3000> DAT3000 
		{
			get
			{
				return this.GetAll<DAT3000>();
			}
		}
		
		public IQueryable<DAT0000> DAT0000 
		{
			get
			{
				return this.GetAll<DAT0000>();
			}
		}
		
		public IQueryable<AppLog> AppLogs 
		{
			get
			{
				return this.GetAll<AppLog>();
			}
		}
		
		public static BackendConfiguration GetBackendConfiguration()
		{
			BackendConfiguration backend = new BackendConfiguration();
			backend.Backend = "MsSql";
			backend.ProviderName = "System.Data.SqlClient";
			backend.Logging.LogEvents = LoggingLevel.Normal;
			backend.Logging.MetricStoreSnapshotInterval = 0;
			backend.Logging.Downloader.Filename = "Log";
			backend.Logging.Downloader.EventText = true;
			return backend;
		}
	}
	
	public interface IEntitiesModelUnitOfWork : IUnitOfWork
	{
		IQueryable<Scanned> Scanneds
		{
			get;
		}
		IQueryable<ScanGroup> ScanGroups
		{
			get;
		}
		IQueryable<DAT9397F> DAT9397Fs
		{
			get;
		}
		IQueryable<DAT8000> DAT8000
		{
			get;
		}
		IQueryable<DAT2000> DAT2000
		{
			get;
		}
		IQueryable<BookTable> BookTables
		{
			get;
		}
		IQueryable<Apoint> Apoints
		{
			get;
		}
		IQueryable<DAT3000> DAT3000
		{
			get;
		}
		IQueryable<DAT0000> DAT0000
		{
			get;
		}
		IQueryable<AppLog> AppLogs
		{
			get;
		}
	}
}
#pragma warning restore 1591
