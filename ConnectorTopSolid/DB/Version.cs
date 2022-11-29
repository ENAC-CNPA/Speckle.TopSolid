using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TK = TopSolid.Kernel;

namespace Speckle.ConnectorTopSolid.DB
{

	/// <summary>
	/// Represents a version for an addin
	/// the version is based on TopSolid Major, Minor, Build and Addin revision
	/// It allows to manage addin versioning independently of TopSolid but keeping a certain coherency with TopSolid
	/// </summary>
	[Serializable]
	public struct Version : IComparable<Version>, IEquatable<Version>, TK.SX.IO.IWritable
	{
		/// <summary>
		/// Current revision version in string format.
		/// </summary>
		public const string CurrentRevisionString = "001";


		/// <summary>
		/// Current version of assemblies in string format.
		/// </summary>
		public const string CurrentAssemblyVersionString = 
			TK.SX.Version.CurrentMajorString + "." +
			TK.SX.Version.CurrentMinorString + "." +
			TK.SX.Version.CurrentBuildString + "." +
			CurrentRevisionString;


		// TopSolid version struct storing the information
		private TK.SX.Version version;


		/// <summary>
		/// Current version.
		/// </summary>
		private static Version current = new Version(CurrentAssemblyVersionString);

		// Static properties:

		/// <summary>
		/// Gets the current version.
		/// </summary>
		public static Version Current
		{
			get
			{
				return current;
			}
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="Version"/> struct.
		/// </summary>
		/// <param name="inMajor">Major number must be within [0,20].</param>
		/// <param name="inMinor">Minor number must be within [0,99].</param>
		/// <param name="inBuild">Build number must be within [0,999].</param>
		/// <param name="inRevision">Revision number must be within [0,999].</param>
		public Version(int inMajor, int inMinor, int inBuild, int inRevision)
		{
			this.version = new TK.SX.Version(
				inMajor,
				inMinor,
				inBuild,
				inRevision
				);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Version"/> struct.
		/// </summary>
		/// <param name="inVersionString">Version string.</param>
		public Version(string inVersionString)
		{
			this.version = new TK.SX.Version(inVersionString);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Version"/> class by reading
		/// data from a stream.
		/// </summary>
		/// <param name="inReader">Reader to use.</param>
		public Version(TK.SX.IO.IReader inReader)
		{
			this.version = new TK.SX.Version(inReader);
			
		}



		/// <inheritdoc/>
		public override bool Equals(object o)
		{
			return (o is Version && this.version.Equals(((Version)o).version));
		}

		/// <inheritdoc/>
		public override int GetHashCode()
		{
			return this.version.GetHashCode();
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return this.version.ToString();
		}

		#region IComparable<Version> Members

		/// <inheritdoc/>
		public bool IsBefore(int inMajor, int inMinor, int inBuild)
		{
			return (this.version.IsBefore(inMajor, inMinor, inBuild));
		}

		/// <inheritdoc/>
		public bool IsAfter(int inMajor, int inMinor, int inBuild)
		{
			return (this.version.IsAfter(inMajor, inMinor, inBuild));
		}

		/// <inheritdoc/>
		public bool IsBeforeRevision( int inRevision)
		{
			return (this.version.Revision<inRevision);
		}

		/// <inheritdoc/>
		public bool IsAfterRevision(int inRevision)
		{
			return (this.version.Revision>inRevision);
		}


		/// <inheritdoc/>
		public int CompareTo(Version inOther)
		{
			if (this.version < inOther.version)
				return -1;
			else if (this.version > inOther.version)
				return 1;
			else
				return 0;
		}

		#endregion

		#region IEquatable<Version> Members

		/// <inheritdoc/>
		public bool Equals(Version inOther)
		{
			return this.version.Equals(inOther.version);
		}

		#endregion

		#region IWritable Members

		/// <inheritdoc/>
		public void Write(TK.SX.IO.IWriter inWriter)
		{
			this.version.Write(inWriter);
		}

		#endregion
	}
}
