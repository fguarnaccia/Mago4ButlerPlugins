using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.Model
{
    public class Version : ICloneable, IComparable
    {
        int major;
        int minor;
        int build;
        int revision;

        public int Major
        {
            get
            {
                return major;
            }
            set
            {
                major = value;
            }
        }

        public int Minor
        {
            get
            {
                return minor;
            }
            set
            {
                minor = value;
            }
        }

        public int Build
        {
            get
            {
                return build;
            }
            set
            {
                build = value;
            }
        }

        public int Revision
        {
            get
            {
                return revision;
            }
            set
            {
                revision = value;
            }
        }

        public Version()
        {
            this.build = 0;
            this.revision = 0;
            this.major = 0;
            this.minor = 0;
        }

        public static Version Parse(string value)
        {
            var ver = System.Version.Parse(value);

            return new Version() { major = ver.Major, minor = ver.Minor, build = ver.Build, revision = ver.Revision };
        }

        public Version(string version)
        {
            this.build = 0;
            this.revision = 0;
            if (version == null)
            {
                throw new ArgumentNullException("version");
            }
            char[] chArray1 = new char[1] { '.' };
            string[] textArray1 = version.Split(chArray1);
            int num1 = textArray1.Length;
            if ((num1 < 2) || (num1 > 4))
            {
                throw new ArgumentException("Arg_VersionString");
            }
            this.major = int.Parse(textArray1[0], CultureInfo.InvariantCulture);
            if (this.major < 0)
            {
                throw new ArgumentOutOfRangeException("version", "ArgumentOutOfRange_Version");
            }
            this.minor = int.Parse(textArray1[1], CultureInfo.InvariantCulture);
            if (this.minor < 0)
            {
                throw new ArgumentOutOfRangeException("version", "ArgumentOutOfRange_Version");
            }
            num1 -= 2;
            if (num1 > 0)
            {
                this.build = int.Parse(textArray1[2], CultureInfo.InvariantCulture);
                if (this.build < 0)
                {
                    throw new ArgumentOutOfRangeException("build", "ArgumentOutOfRange_Version");
                }
                num1--;
                if (num1 > 0)
                {
                    this.revision = int.Parse(textArray1[3], CultureInfo.InvariantCulture);
                    if (this.revision < 0)
                    {
                        throw new ArgumentOutOfRangeException("revision", "ArgumentOutOfRange_Version");
                    }
                }
            }
        }

        public Version(int major, int minor)
        {
            this.build = 0;
            this.revision = 0;
            if (major < 0)
            {
                throw new ArgumentOutOfRangeException("major", "ArgumentOutOfRange_Version");
            }
            if (minor < 0)
            {
                throw new ArgumentOutOfRangeException("minor", "ArgumentOutOfRange_Version");
            }
            this.major = major;
            this.minor = minor;
            this.major = major;
        }

        public Version(int major, int minor, int build)
        {
            this.build = 0;
            this.revision = 0;
            if (major < 0)
            {
                throw new ArgumentOutOfRangeException("major", "ArgumentOutOfRange_Version");
            }
            if (minor < 0)
            {
                throw new ArgumentOutOfRangeException("minor", "ArgumentOutOfRange_Version");
            }
            if (build < 0)
            {
                throw new ArgumentOutOfRangeException("build", "ArgumentOutOfRange_Version");
            }
            this.major = major;
            this.minor = minor;
            this.build = build;
        }

        public Version(int major, int minor, int build, int revision)
        {
            this.build = 0;
            this.revision = 0;
            if (major < 0)
            {
                throw new ArgumentOutOfRangeException("major", "ArgumentOutOfRange_Version");
            }
            if (minor < 0)
            {
                throw new ArgumentOutOfRangeException("minor", "ArgumentOutOfRange_Version");
            }
            if (build < 0)
            {
                throw new ArgumentOutOfRangeException("build", "ArgumentOutOfRange_Version");
            }
            if (revision < 0)
            {
                throw new ArgumentOutOfRangeException("revision", "ArgumentOutOfRange_Version");
            }
            this.major = major;
            this.minor = minor;
            this.build = build;
            this.revision = revision;
        }

        public object Clone()
        {
            var version1 = new Version();
            version1.major = this.major;
            version1.minor = this.minor;
            version1.build = this.build;
            version1.revision = this.revision;
            return version1;
        }

        public int CompareTo(object version)
        {
            if (version == null)
            {
                return 1;
            }
            if (!(version is Version))
            {
                throw new ArgumentException("Arg_MustBeVersion");
            }
            var version1 = (Version)version;
            if (this.major != version1.Major)
            {
                if (this.major > version1.Major)
                {
                    return 1;
                }
                return -1;
            }
            if (this.minor != version1.Minor)
            {
                if (this.minor > version1.Minor)
                {
                    return 1;
                }
                return -1;
            }
            if (this.build != version1.Build)
            {
                if (this.build > version1.Build)
                {
                    return 1;
                }
                return -1;
            }
            if (this.revision == version1.Revision)
            {
                return 0;
            }
            if (this.revision > version1.Revision)
            {
                return 1;
            }
            return -1;
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !(obj is Version))
            {
                return false;
            }
            Version version1 = (Version)obj;
            if (((this.major == version1.Major) && (this.minor == version1.Minor)) && (this.build == version1.Build) && (this.revision == version1.Revision))
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            int num1 = 0;
            num1 |= ((this.major & 15) << 0x1c);
            num1 |= ((this.minor & 0xff) << 20);
            num1 |= ((this.build & 0xff) << 12);
            return (num1 | this.revision & 0xfff);
        }

        public static bool operator ==(Version v1, Version v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator >(Version v1, Version v2)
        {
            return (v2 < v1);
        }

        public static bool operator >=(Version v1, Version v2)
        {
            return (v2 <= v1);
        }

        public static bool operator !=(Version v1, Version v2)
        {
            return !(v1.Equals(v2));
        }

        public static bool operator <(Version v1, Version v2)
        {
            if (v1 == null)
            {
                throw new ArgumentNullException("v1");
            }
            return (v1.CompareTo(v2) < 0);
        }

        public static bool operator <=(Version v1, Version v2)
        {
            if (v1 == null)
            {
                throw new ArgumentNullException("v1");
            }
            return (v1.CompareTo(v2) <= 0);
        }

        public override string ToString()
        {
            if (this.build == -1)
            {
                return this.ToString(2);
            }
            if (this.revision == -1)
            {
                return this.ToString(3);
            }
            return this.ToString(4);
        }

        public string ToString(int fieldCount)
        {
            object[] objArray1;
            switch (fieldCount)
            {
                case 0:
                    {
                        return string.Empty;
                    }
                case 1:
                    {
                        return (this.major.ToString());
                    }
                case 2:
                    {
                        return (this.major.ToString() + "." + this.minor.ToString());
                    }
            }
            if (this.build == -1)
            {
                throw new ArgumentException(string.Format("ArgumentOutOfRange_Bounds_Lower_Upper {0},{1}", "0", "2"), "fieldCount");
            }
            if (fieldCount == 3)
            {
                objArray1 = new object[5] { this.major, ".", this.minor, ".", this.build };
                return string.Concat(objArray1);
            }
            if (this.revision == -1)
            {
                throw new ArgumentException(string.Format("ArgumentOutOfRange_Bounds_Lower_Upper {0},{1}", "0", "3"), "fieldCount");
            }
            if (fieldCount == 4)
            {
                objArray1 = new object[7] { this.major, ".", this.minor, ".", this.build, ".", this.revision };
                return string.Concat(objArray1);
            }
            throw new ArgumentException(string.Format("ArgumentOutOfRange_Bounds_Lower_Upper {0},{1}", "0", "4"), "fieldCount");
        }
    }
}
