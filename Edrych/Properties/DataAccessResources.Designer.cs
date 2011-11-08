﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Edrych.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class DataAccessResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal DataAccessResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Edrych.Properties.DataAccessResources", typeof(DataAccessResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select
        ///	TABLE_NAME
        ///from
        ///	INFORMATION_SCHEMA.TABLES
        ///order by
        ///	1.
        /// </summary>
        internal static string ANSI_FindTables {
            get {
                return ResourceManager.GetString("ANSI_FindTables", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select
        ///	TABLE_NAME
        ///from
        ///	INFORMATION_SCHEMA.VIEWS
        ///order by
        ///	1.
        /// </summary>
        internal static string ANSI_FindViews {
            get {
                return ResourceManager.GetString("ANSI_FindViews", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Data Source=%TEMP%\Test.db3.
        /// </summary>
        internal static string DefaultConnectionString {
            get {
                return ResourceManager.GetString("DefaultConnectionString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PRAGMA table_info(@TableName).
        /// </summary>
        internal static string SQLite_FindColumns {
            get {
                return ResourceManager.GetString("SQLite_FindColumns", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT name FROM sqlite_master
        ///WHERE type=&apos;table&apos;
        ///ORDER BY name;.
        /// </summary>
        internal static string SQLite_FindTables {
            get {
                return ResourceManager.GetString("SQLite_FindTables", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT name FROM sqlite_master
        ///WHERE type=&apos;view&apos;
        ///ORDER BY name;.
        /// </summary>
        internal static string SQLite_FindViews {
            get {
                return ResourceManager.GetString("SQLite_FindViews", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select
        ///	COLUMN_NAME as name,
        ///	DATA_TYPE 
        ///		+ ISNULL(&apos;(&apos; + cast(character_maximum_length as varchar) + &apos;)&apos;, &apos;&apos;)
        ///		+ case 
        ///			when DATA_TYPE in (&apos;decimal&apos;, &apos;numeric&apos;) then
        ///				ISNULL(&apos;(&apos; + cast(NUMERIC_PRECISION as varchar) + &apos;, &apos; + CAST(NUMERIC_SCALE as varchar) + &apos;)&apos;, &apos;&apos;)
        ///			else &apos;&apos;
        ///		  end
        ///	as type,
        ///	IS_NULLABLE
        ///from
        ///	INFORMATION_SCHEMA.COLUMNS
        ///where
        ///	TABLE_NAME = @TableName
        ///order by
        ///	ORDINAL_POSITION.
        /// </summary>
        internal static string SQLServer_FindColumns {
            get {
                return ResourceManager.GetString("SQLServer_FindColumns", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select
        ///	name
        ///from
        ///	sys.databases
        ///order by
        ///	1.
        /// </summary>
        internal static string SQLServer_FindDatabases {
            get {
                return ResourceManager.GetString("SQLServer_FindDatabases", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to if exists (select 1 from sys.databases d where d.name = @DatabaseName)
        ///begin
        ///
        ///use @DatabaseReplaceName
        ///
        ///end.
        /// </summary>
        internal static string SQLServer_SetDatabase {
            get {
                return ResourceManager.GetString("SQLServer_SetDatabase", resourceCulture);
            }
        }
    }
}