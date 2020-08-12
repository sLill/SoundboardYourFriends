﻿using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Reflection;

//namespace NAudio.CoreAudioApi
namespace SoundboardYourFriends.Core
{
    /// <summary>
    /// PROPERTYKEY is defined in wtypes.h
    /// </summary>
    public struct WindowsDevicePropertyKeys
    {
        /// <summary>
        /// Format ID
        /// </summary>
        public Guid formatId;
        /// <summary>
        /// Property ID
        /// </summary>
        public int propertyId;

        // http://msdn.microsoft.com/en-us/library/windows/desktop/ff384862(v=vs.85).aspx
        // https://subversion.assembla.com/svn/portaudio/portaudio/trunk/src/hostapi/wasapi/mingw-include/propkey.h

        public WindowsDevicePropertyKeys(Guid guid, int propertyId)
        {
            this.formatId = guid;
            this.propertyId = propertyId;
        }
        public WindowsDevicePropertyKeys(string formatId, int propertyId)
            : this(new Guid(formatId), propertyId) { }
        
        public WindowsDevicePropertyKeys(uint a, uint b, uint c, uint d, uint e, uint f, uint g, uint h, uint i, uint j, uint k, int propertyId)
            : this(new Guid((uint)a, (ushort)b, (ushort)c, (byte)d, (byte)e, (byte)f, (byte)g, (byte)h, (byte)i, (byte)j, (byte)k), propertyId) { }
        public string GetBaseString()
        {
            return string.Format("{0},{1}", formatId.ToString(), propertyId.ToString());
        }

        public PropertyKey ToNAudioPropertyKey() => new PropertyKey(formatId, propertyId);

        public override string ToString()
        {
            try
            {
                string basekey;
                WindowsDevicePropertyKeys val;
                if (predefinedkeys == null)
                {
                    predefinedkeys = new Dictionary<string, string>();
                    foreach (KeyValuePair<string, WindowsDevicePropertyKeys> kvp in typeof(WindowsDevicePropertyKeys).ListTypes<WindowsDevicePropertyKeys>())
                    {
                        val = kvp.Value;
                        basekey = val.GetBaseString();
                        if (!predefinedkeys.ContainsKey(basekey))
                            predefinedkeys.Add(basekey, kvp.Key);
                    }
                }
                basekey = this.GetBaseString();
                if (predefinedkeys.ContainsKey(basekey))
                    return predefinedkeys[basekey]; // return "PKEY_Device_DeviceDesc" if known

                return basekey; // otherwise, return "a45c254e-df1c-4efd-8020-67d146a850e0,2"
            }
            catch (Exception)
            {
                return string.Format("{0}.{1}", formatId.ToString(), propertyId.ToString());
            }
        }

        //sample ("a45c254e-df1c-4efd-8020-67d146a850e0,2", "PKEY_Device_DeviceDesc")
        static Dictionary<string, string> predefinedkeys = null;
        //
        // Device properties
        // These PKEYs correspond to the old setupapi SPDRP_XXX properties
        //
        #region Device Properties
        public static WindowsDevicePropertyKeys PKEY_Device_DeviceDesc = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 2);     // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_Device_HardwareIds = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 3);     // DEVPROP_TYPE_STRING_LIST
        public static WindowsDevicePropertyKeys PKEY_Device_CompatibleIds = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 4);     // DEVPROP_TYPE_STRING_LIST
        public static WindowsDevicePropertyKeys PKEY_Device_Service = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 6);     // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_Device_Class = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 9);     // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_Device_ClassGuid = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 10);    // DEVPROP_TYPE_GUID
        public static WindowsDevicePropertyKeys PKEY_Device_Driver = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 11);    // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_Device_ConfigFlags = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 12);    // DEVPROP_TYPE_UINT32
        public static WindowsDevicePropertyKeys PKEY_Device_Manufacturer = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 13);    // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_Device_FriendlyName = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 14);    // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_Device_LocationInfo = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 15);    // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_Device_PDOName = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 16);    // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_Device_Capabilities = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 17);    // DEVPROP_TYPE_UNINT32
        public static WindowsDevicePropertyKeys PKEY_Device_UINumber = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 18);    // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_Device_UpperFilters = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 19);    // DEVPROP_TYPE_STRING_LIST
        public static WindowsDevicePropertyKeys PKEY_Device_LowerFilters = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 20);    // DEVPROP_TYPE_STRING_LIST
        public static WindowsDevicePropertyKeys PKEY_Device_BusTypeGuid = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 21);    // DEVPROP_TYPE_GUID
        public static WindowsDevicePropertyKeys PKEY_Device_LegacyBusType = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 22);    // DEVPROP_TYPE_UINT32
        public static WindowsDevicePropertyKeys PKEY_Device_BusNumber = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 23);    // DEVPROP_TYPE_UINT32
        public static WindowsDevicePropertyKeys PKEY_Device_EnumeratorName = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 24);    // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_Device_Security = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 25);    // DEVPROP_TYPE_SECURITY_DESCRIPTOR
        public static WindowsDevicePropertyKeys PKEY_Device_SecuritySDS = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 26);    // DEVPROP_TYPE_SECURITY_DESCRIPTOR_STRING
        public static WindowsDevicePropertyKeys PKEY_Device_DevType = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 27);    // DEVPROP_TYPE_UINT32
        public static WindowsDevicePropertyKeys PKEY_Device_Exclusive = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 28);    // DEVPROP_TYPE_UINT32
        public static WindowsDevicePropertyKeys PKEY_Device_Characteristics = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 29);    // DEVPROP_TYPE_UINT32
        public static WindowsDevicePropertyKeys PKEY_Device_Address = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 30);    // DEVPROP_TYPE_UINT32
        public static WindowsDevicePropertyKeys PKEY_Device_UINumberDescFormat = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 31);    // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_Device_PowerData = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 32);    // DEVPROP_TYPE_BINARY
        public static WindowsDevicePropertyKeys PKEY_Device_RemovalPolicy = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 33);    // DEVPROP_TYPE_UINT32
        public static WindowsDevicePropertyKeys PKEY_Device_RemovalPolicyDefault = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 34);    // DEVPROP_TYPE_UINT32
        public static WindowsDevicePropertyKeys PKEY_Device_RemovalPolicyOverride = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 35);    // DEVPROP_TYPE_UINT32
        public static WindowsDevicePropertyKeys PKEY_Device_InstallState = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 36);    // DEVPROP_TYPE_UINT32
        public static WindowsDevicePropertyKeys PKEY_Device_LocationPaths = new WindowsDevicePropertyKeys(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 37);    // DEVPROP_TYPE_STRING_LIST
                                                                                                                                                                  //
                                                                                                                                                                  // Device properties
                                                                                                                                                                  // These PKEYs correspond to a device's status and problem code
                                                                                                                                                                  //
        public static WindowsDevicePropertyKeys PKEY_Device_DevNodeStatus = new WindowsDevicePropertyKeys(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7, 2);     // DEVPROP_TYPE_UINT32
        public static WindowsDevicePropertyKeys PKEY_Device_ProblemCode = new WindowsDevicePropertyKeys(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7, 3);     // DEVPROP_TYPE_UINT32
                                                                                                                                                                //
                                                                                                                                                                // Device properties
                                                                                                                                                                // These PKEYs correspond to device relations
                                                                                                                                                                //
        public static WindowsDevicePropertyKeys PKEY_Device_EjectionRelations = new WindowsDevicePropertyKeys(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7, 4);     // DEVPROP_TYPE_STRING_LIST
        public static WindowsDevicePropertyKeys PKEY_Device_RemovalRelations = new WindowsDevicePropertyKeys(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7, 5);     // DEVPROP_TYPE_STRING_LIST
        public static WindowsDevicePropertyKeys PKEY_Device_PowerRelations = new WindowsDevicePropertyKeys(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7, 6);     // DEVPROP_TYPE_STRING_LIST
        public static WindowsDevicePropertyKeys PKEY_Device_BusRelations = new WindowsDevicePropertyKeys(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7, 7);     // DEVPROP_TYPE_STRING_LIST
        public static WindowsDevicePropertyKeys PKEY_Device_Parent = new WindowsDevicePropertyKeys(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7, 8);     // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_Device_Children = new WindowsDevicePropertyKeys(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7, 9);     // DEVPROP_TYPE_STRING_LIST
        public static WindowsDevicePropertyKeys PKEY_Device_Siblings = new WindowsDevicePropertyKeys(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7, 10);    // DEVPROP_TYPE_STRING_LIST
                                                                                                                                                             //
                                                                                                                                                             // Other Device properties
                                                                                                                                                             //
        public static WindowsDevicePropertyKeys PKEY_Device_Reported = new WindowsDevicePropertyKeys(0x80497100, 0x8c73, 0x48b9, 0xaa, 0xd9, 0xce, 0x38, 0x7e, 0x19, 0xc5, 0x6e, 2);     // DEVPROP_TYPE_BOOLEAN
        public static WindowsDevicePropertyKeys PKEY_Device_Legacy = new WindowsDevicePropertyKeys(0x80497100, 0x8c73, 0x48b9, 0xaa, 0xd9, 0xce, 0x38, 0x7e, 0x19, 0xc5, 0x6e, 3);     // DEVPROP_TYPE_BOOLEAN
        public static WindowsDevicePropertyKeys PKEY_Device_InstanceId = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 256);   // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_Numa_Proximity_Domain = new WindowsDevicePropertyKeys(0x540b947e, 0x8b40, 0x45bc, 0xa8, 0xa2, 0x6a, 0x0b, 0x89, 0x4c, 0xbd, 0xa2, 1);     // DEVPROP_TYPE_UINT32
                                                                                                                                                                   //
                                                                                                                                                                   // Device driver properties
                                                                                                                                                                   //
        public static WindowsDevicePropertyKeys PKEY_Device_DriverDate = new WindowsDevicePropertyKeys(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6, 2);      // DEVPROP_TYPE_FILETIME
        public static WindowsDevicePropertyKeys PKEY_Device_DriverVersion = new WindowsDevicePropertyKeys(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6, 3);      // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_Device_DriverDesc = new WindowsDevicePropertyKeys(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6, 4);      // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_Device_DriverInfPath = new WindowsDevicePropertyKeys(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6, 5);      // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_Device_DriverInfSection = new WindowsDevicePropertyKeys(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6, 6);      // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_Device_DriverInfSectionExt = new WindowsDevicePropertyKeys(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6, 7);      // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_Device_MatchingDeviceId = new WindowsDevicePropertyKeys(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6, 8);      // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_Device_DriverProvider = new WindowsDevicePropertyKeys(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6, 9);      // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_Device_DriverPropPageProvider = new WindowsDevicePropertyKeys(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6, 10);     // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_Device_DriverCoInstallers = new WindowsDevicePropertyKeys(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6, 11);     // DEVPROP_TYPE_STRING_LIST
        public static WindowsDevicePropertyKeys PKEY_Device_ResourcePickerTags = new WindowsDevicePropertyKeys(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6, 12);     // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_Device_ResourcePickerExceptions = new WindowsDevicePropertyKeys(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6, 13); // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_Device_DriverRank = new WindowsDevicePropertyKeys(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6, 14);     // DEVPROP_TYPE_UINT32
        public static WindowsDevicePropertyKeys PKEY_Device_DriverLogoLevel = new WindowsDevicePropertyKeys(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6, 15);     // DEVPROP_TYPE_UINT32
        public static WindowsDevicePropertyKeys PKEY_Device_NoConnectSound = new WindowsDevicePropertyKeys(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6, 17);     // DEVPROP_TYPE_BOOLEAN
        public static WindowsDevicePropertyKeys PKEY_Device_GenericDriverInstalled = new WindowsDevicePropertyKeys(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6, 18);     // DEVPROP_TYPE_BOOLEAN
                                                                                                                                                                           //
                                                                                                                                                                           // Device properties that were set by the driver package that was installed
                                                                                                                                                                           // on the device.
                                                                                                                                                                           //
        public static WindowsDevicePropertyKeys PKEY_DrvPkg_Model = new WindowsDevicePropertyKeys(0xcf73bb51, 0x3abf, 0x44a2, 0x85, 0xe0, 0x9a, 0x3d, 0xc7, 0xa1, 0x21, 0x32, 2);     // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_DrvPkg_VendorWebSite = new WindowsDevicePropertyKeys(0xcf73bb51, 0x3abf, 0x44a2, 0x85, 0xe0, 0x9a, 0x3d, 0xc7, 0xa1, 0x21, 0x32, 3);     // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_DrvPkg_DetailedDescription = new WindowsDevicePropertyKeys(0xcf73bb51, 0x3abf, 0x44a2, 0x85, 0xe0, 0x9a, 0x3d, 0xc7, 0xa1, 0x21, 0x32, 4);     // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_DrvPkg_DocumentationLink = new WindowsDevicePropertyKeys(0xcf73bb51, 0x3abf, 0x44a2, 0x85, 0xe0, 0x9a, 0x3d, 0xc7, 0xa1, 0x21, 0x32, 5);     // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_DrvPkg_Icon = new WindowsDevicePropertyKeys(0xcf73bb51, 0x3abf, 0x44a2, 0x85, 0xe0, 0x9a, 0x3d, 0xc7, 0xa1, 0x21, 0x32, 6);     // DEVPROP_TYPE_STRING_LIST
        public static WindowsDevicePropertyKeys PKEY_DrvPkg_BrandingIcon = new WindowsDevicePropertyKeys(0xcf73bb51, 0x3abf, 0x44a2, 0x85, 0xe0, 0x9a, 0x3d, 0xc7, 0xa1, 0x21, 0x32, 7);     // DEVPROP_TYPE_STRING_LIST
                                                                                                                                                                 //
                                                                                                                                                                 // Device setup class properties
                                                                                                                                                                 // These PKEYs correspond to the old setupapi SPCRP_XXX properties
                                                                                                                                                                 //
        public static WindowsDevicePropertyKeys PKEY_DeviceClass_UpperFilters = new WindowsDevicePropertyKeys(0x4321918b, 0xf69e, 0x470d, 0xa5, 0xde, 0x4d, 0x88, 0xc7, 0x5a, 0xd2, 0x4b, 19);    // DEVPROP_TYPE_STRING_LIST
        public static WindowsDevicePropertyKeys PKEY_DeviceClass_LowerFilters = new WindowsDevicePropertyKeys(0x4321918b, 0xf69e, 0x470d, 0xa5, 0xde, 0x4d, 0x88, 0xc7, 0x5a, 0xd2, 0x4b, 20);    // DEVPROP_TYPE_STRING_LIST
        public static WindowsDevicePropertyKeys PKEY_DeviceClass_Security = new WindowsDevicePropertyKeys(0x4321918b, 0xf69e, 0x470d, 0xa5, 0xde, 0x4d, 0x88, 0xc7, 0x5a, 0xd2, 0x4b, 25);    // DEVPROP_TYPE_SECURITY_DESCRIPTOR
        public static WindowsDevicePropertyKeys PKEY_DeviceClass_SecuritySDS = new WindowsDevicePropertyKeys(0x4321918b, 0xf69e, 0x470d, 0xa5, 0xde, 0x4d, 0x88, 0xc7, 0x5a, 0xd2, 0x4b, 26);    // DEVPROP_TYPE_SECURITY_DESCRIPTOR_STRING
        public static WindowsDevicePropertyKeys PKEY_DeviceClass_DevType = new WindowsDevicePropertyKeys(0x4321918b, 0xf69e, 0x470d, 0xa5, 0xde, 0x4d, 0x88, 0xc7, 0x5a, 0xd2, 0x4b, 27);    // DEVPROP_TYPE_UINT32
        public static WindowsDevicePropertyKeys PKEY_DeviceClass_Exclusive = new WindowsDevicePropertyKeys(0x4321918b, 0xf69e, 0x470d, 0xa5, 0xde, 0x4d, 0x88, 0xc7, 0x5a, 0xd2, 0x4b, 28);    // DEVPROP_TYPE_UINT32
        public static WindowsDevicePropertyKeys PKEY_DeviceClass_Characteristics = new WindowsDevicePropertyKeys(0x4321918b, 0xf69e, 0x470d, 0xa5, 0xde, 0x4d, 0x88, 0xc7, 0x5a, 0xd2, 0x4b, 29);    // DEVPROP_TYPE_UINT32
                                                                                                                                                                         //
                                                                                                                                                                         // Device setup class properties
                                                                                                                                                                         // These PKEYs correspond to registry values under the device class GUID key
                                                                                                                                                                         //
        public static WindowsDevicePropertyKeys PKEY_DeviceClass_Name = new WindowsDevicePropertyKeys(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66, 2);  // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_DeviceClass_ClassName = new WindowsDevicePropertyKeys(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66, 3);  // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_DeviceClass_Icon = new WindowsDevicePropertyKeys(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66, 4);  // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_DeviceClass_ClassInstaller = new WindowsDevicePropertyKeys(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66, 5);  // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_DeviceClass_PropPageProvider = new WindowsDevicePropertyKeys(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66, 6);  // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_DeviceClass_NoInstallClass = new WindowsDevicePropertyKeys(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66, 7);  // DEVPROP_TYPE_BOOLEAN
        public static WindowsDevicePropertyKeys PKEY_DeviceClass_NoDisplayClass = new WindowsDevicePropertyKeys(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66, 8);  // DEVPROP_TYPE_BOOLEAN
        public static WindowsDevicePropertyKeys PKEY_DeviceClass_SilentInstall = new WindowsDevicePropertyKeys(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66, 9);  // DEVPROP_TYPE_BOOLEAN
        public static WindowsDevicePropertyKeys PKEY_DeviceClass_NoUseClass = new WindowsDevicePropertyKeys(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66, 10); // DEVPROP_TYPE_BOOLEAN
        public static WindowsDevicePropertyKeys PKEY_DeviceClass_DefaultService = new WindowsDevicePropertyKeys(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66, 11); // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_DeviceClass_IconPath = new WindowsDevicePropertyKeys(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66, 12); // DEVPROP_TYPE_STRING_LIST
                                                                                                                                                              //
                                                                                                                                                              // Other Device setup class properties
                                                                                                                                                              //
        public static WindowsDevicePropertyKeys PKEY_DeviceClass_ClassCoInstallers = new WindowsDevicePropertyKeys(0x713d1703, 0xa2e2, 0x49f5, 0x92, 0x14, 0x56, 0x47, 0x2e, 0xf3, 0xda, 0x5c, 2); // DEVPROP_TYPE_STRING_LIST
                                                                                                                                                                       //
                                                                                                                                                                       // Device interface properties
                                                                                                                                                                       //
        public static WindowsDevicePropertyKeys PKEY_DeviceInterface_FriendlyName = new WindowsDevicePropertyKeys(0x026e516e, 0xb814, 0x414b, 0x83, 0xcd, 0x85, 0x6d, 0x6f, 0xef, 0x48, 0x22, 2); // DEVPROP_TYPE_STRING
        public static WindowsDevicePropertyKeys PKEY_DeviceInterface_Enabled = new WindowsDevicePropertyKeys(0x026e516e, 0xb814, 0x414b, 0x83, 0xcd, 0x85, 0x6d, 0x6f, 0xef, 0x48, 0x22, 3); // DEVPROP_TYPE_BOOLEAN
        public static WindowsDevicePropertyKeys PKEY_DeviceInterface_ClassGuid = new WindowsDevicePropertyKeys(0x026e516e, 0xb814, 0x414b, 0x83, 0xcd, 0x85, 0x6d, 0x6f, 0xef, 0x48, 0x22, 4); // DEVPROP_TYPE_GUID
                                                                                                                                                                   //
                                                                                                                                                                   // Device interface class properties
                                                                                                                                                                   //
        public static WindowsDevicePropertyKeys PKEY_DeviceInterfaceClass_DefaultInterface = new WindowsDevicePropertyKeys(0x14c83a99, 0x0b3f, 0x44b7, 0xbe, 0x4c, 0xa1, 0x78, 0xd3, 0x99, 0x05, 0x64, 2); // DEVPROP_TYPE_STRING

        public static WindowsDevicePropertyKeys PKEY_Audio_ChannelCount = new WindowsDevicePropertyKeys(0x64440490, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 7);
        public static WindowsDevicePropertyKeys PKEY_Audio_Compression = new WindowsDevicePropertyKeys(0x64440490, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 10);
        public static WindowsDevicePropertyKeys PKEY_Audio_EncodingBitrate = new WindowsDevicePropertyKeys(0x64440490, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 4);
        public static WindowsDevicePropertyKeys PKEY_Audio_Format = new WindowsDevicePropertyKeys(0x64440490, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 2);
        public static WindowsDevicePropertyKeys PKEY_Audio_IsVariableBitRate = new WindowsDevicePropertyKeys(0xE6822FEE, 0x8C17, 0x4D62, 0x82, 0x3C, 0x8E, 0x9C, 0xFC, 0xBD, 0x1D, 0x5C, 100);
        public static WindowsDevicePropertyKeys PKEY_Audio_PeakValue = new WindowsDevicePropertyKeys(0x2579E5D0, 0x1116, 0x4084, 0xBD, 0x9A, 0x9B, 0x4F, 0x7C, 0xB4, 0xDF, 0x5E, 100);
        public static WindowsDevicePropertyKeys PKEY_Audio_SampleRate = new WindowsDevicePropertyKeys(0x64440490, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 5);
        public static WindowsDevicePropertyKeys PKEY_Audio_SampleSize = new WindowsDevicePropertyKeys(0x64440490, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 6);
        public static WindowsDevicePropertyKeys PKEY_Audio_StreamName = new WindowsDevicePropertyKeys(0x64440490, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 9);
        public static WindowsDevicePropertyKeys PKEY_Audio_StreamNumber = new WindowsDevicePropertyKeys(0x64440490, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 8);
        public static WindowsDevicePropertyKeys PKEY_Calendar_Duration = new WindowsDevicePropertyKeys(0x293CA35A, 0x09AA, 0x4DD2, 0xB1, 0x80, 0x1F, 0xE2, 0x45, 0x72, 0x8A, 0x52, 100);
        public static WindowsDevicePropertyKeys PKEY_Calendar_IsOnline = new WindowsDevicePropertyKeys(0xBFEE9149, 0xE3E2, 0x49A7, 0xA8, 0x62, 0xC0, 0x59, 0x88, 0x14, 0x5C, 0xEC, 100);
        public static WindowsDevicePropertyKeys PKEY_Calendar_IsRecurring = new WindowsDevicePropertyKeys(0x315B9C8D, 0x80A9, 0x4EF9, 0xAE, 0x16, 0x8E, 0x74, 0x6D, 0xA5, 0x1D, 0x70, 100);
        public static WindowsDevicePropertyKeys PKEY_Calendar_Location = new WindowsDevicePropertyKeys(0xF6272D18, 0xCECC, 0x40B1, 0xB2, 0x6A, 0x39, 0x11, 0x71, 0x7A, 0xA7, 0xBD, 100);
        public static WindowsDevicePropertyKeys PKEY_Calendar_OptionalAttendeeAddresses = new WindowsDevicePropertyKeys(0xD55BAE5A, 0x3892, 0x417A, 0xA6, 0x49, 0xC6, 0xAC, 0x5A, 0xAA, 0xEA, 0xB3, 100);
        public static WindowsDevicePropertyKeys PKEY_Calendar_OptionalAttendeeNames = new WindowsDevicePropertyKeys(0x09429607, 0x582D, 0x437F, 0x84, 0xC3, 0xDE, 0x93, 0xA2, 0xB2, 0x4C, 0x3C, 100);
        public static WindowsDevicePropertyKeys PKEY_Calendar_OrganizerAddress = new WindowsDevicePropertyKeys(0x744C8242, 0x4DF5, 0x456C, 0xAB, 0x9E, 0x01, 0x4E, 0xFB, 0x90, 0x21, 0xE3, 100);
        public static WindowsDevicePropertyKeys PKEY_Calendar_OrganizerName = new WindowsDevicePropertyKeys(0xAAA660F9, 0x9865, 0x458E, 0xB4, 0x84, 0x01, 0xBC, 0x7F, 0xE3, 0x97, 0x3E, 100);
        public static WindowsDevicePropertyKeys PKEY_Calendar_ReminderTime = new WindowsDevicePropertyKeys(0x72FC5BA4, 0x24F9, 0x4011, 0x9F, 0x3F, 0xAD, 0xD2, 0x7A, 0xFA, 0xD8, 0x18, 100);
        public static WindowsDevicePropertyKeys PKEY_Calendar_RequiredAttendeeAddresses = new WindowsDevicePropertyKeys(0x0BA7D6C3, 0x568D, 0x4159, 0xAB, 0x91, 0x78, 0x1A, 0x91, 0xFB, 0x71, 0xE5, 100);
        public static WindowsDevicePropertyKeys PKEY_Calendar_RequiredAttendeeNames = new WindowsDevicePropertyKeys(0xB33AF30B, 0xF552, 0x4584, 0x93, 0x6C, 0xCB, 0x93, 0xE5, 0xCD, 0xA2, 0x9F, 100);
        public static WindowsDevicePropertyKeys PKEY_Calendar_Resources = new WindowsDevicePropertyKeys(0x00F58A38, 0xC54B, 0x4C40, 0x86, 0x96, 0x97, 0x23, 0x59, 0x80, 0xEA, 0xE1, 100);
        public static WindowsDevicePropertyKeys PKEY_Calendar_ShowTimeAs = new WindowsDevicePropertyKeys(0x5BF396D4, 0x5EB2, 0x466F, 0xBD, 0xE9, 0x2F, 0xB3, 0xF2, 0x36, 0x1D, 0x6E, 100);
        public static WindowsDevicePropertyKeys PKEY_Calendar_ShowTimeAsText = new WindowsDevicePropertyKeys(0x53DA57CF, 0x62C0, 0x45C4, 0x81, 0xDE, 0x76, 0x10, 0xBC, 0xEF, 0xD7, 0xF5, 100);
        public static WindowsDevicePropertyKeys PKEY_Communication_AccountName = new WindowsDevicePropertyKeys(0xE3E0584C, 0xB788, 0x4A5A, 0xBB, 0x20, 0x7F, 0x5A, 0x44, 0xC9, 0xAC, 0xDD, 9);
        public static WindowsDevicePropertyKeys PKEY_Communication_Suffix = new WindowsDevicePropertyKeys(0x807B653A, 0x9E91, 0x43EF, 0x8F, 0x97, 0x11, 0xCE, 0x04, 0xEE, 0x20, 0xC5, 100);
        public static WindowsDevicePropertyKeys PKEY_Communication_TaskStatus = new WindowsDevicePropertyKeys(0xBE1A72C6, 0x9A1D, 0x46B7, 0xAF, 0xE7, 0xAF, 0xAF, 0x8C, 0xEF, 0x49, 0x99, 100);
        public static WindowsDevicePropertyKeys PKEY_Communication_TaskStatusText = new WindowsDevicePropertyKeys(0xA6744477, 0xC237, 0x475B, 0xA0, 0x75, 0x54, 0xF3, 0x44, 0x98, 0x29, 0x2A, 100);
        public static WindowsDevicePropertyKeys PKEY_Computer_DecoratedFreeSpace = new WindowsDevicePropertyKeys(0x9B174B35, 0x40FF, 0x11D2, 0xA2, 0x7E, 0x00, 0xC0, 0x4F, 0xC3, 0x08, 0x71, 7);
        public static WindowsDevicePropertyKeys PKEY_Contact_Anniversary = new WindowsDevicePropertyKeys(0x9AD5BADB, 0xCEA7, 0x4470, 0xA0, 0x3D, 0xB8, 0x4E, 0x51, 0xB9, 0x94, 0x9E, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_AssistantName = new WindowsDevicePropertyKeys(0xCD102C9C, 0x5540, 0x4A88, 0xA6, 0xF6, 0x64, 0xE4, 0x98, 0x1C, 0x8C, 0xD1, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_AssistantTelephone = new WindowsDevicePropertyKeys(0x9A93244D, 0xA7AD, 0x4FF8, 0x9B, 0x99, 0x45, 0xEE, 0x4C, 0xC0, 0x9A, 0xF6, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_Birthday = new WindowsDevicePropertyKeys(0x176DC63C, 0x2688, 0x4E89, 0x81, 0x43, 0xA3, 0x47, 0x80, 0x0F, 0x25, 0xE9, 47);
        public static WindowsDevicePropertyKeys PKEY_Contact_BusinessAddress = new WindowsDevicePropertyKeys(0x730FB6DD, 0xCF7C, 0x426B, 0xA0, 0x3F, 0xBD, 0x16, 0x6C, 0xC9, 0xEE, 0x24, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_BusinessAddressCity = new WindowsDevicePropertyKeys(0x402B5934, 0xEC5A, 0x48C3, 0x93, 0xE6, 0x85, 0xE8, 0x6A, 0x2D, 0x93, 0x4E, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_BusinessAddressCountry = new WindowsDevicePropertyKeys(0xB0B87314, 0xFCF6, 0x4FEB, 0x8D, 0xFF, 0xA5, 0x0D, 0xA6, 0xAF, 0x56, 0x1C, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_BusinessAddressPostalCode = new WindowsDevicePropertyKeys(0xE1D4A09E, 0xD758, 0x4CD1, 0xB6, 0xEC, 0x34, 0xA8, 0xB5, 0xA7, 0x3F, 0x80, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_BusinessAddressPostOfficeBox = new WindowsDevicePropertyKeys(0xBC4E71CE, 0x17F9, 0x48D5, 0xBE, 0xE9, 0x02, 0x1D, 0xF0, 0xEA, 0x54, 0x09, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_BusinessAddressState = new WindowsDevicePropertyKeys(0x446F787F, 0x10C4, 0x41CB, 0xA6, 0xC4, 0x4D, 0x03, 0x43, 0x55, 0x15, 0x97, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_BusinessAddressStreet = new WindowsDevicePropertyKeys(0xDDD1460F, 0xC0BF, 0x4553, 0x8C, 0xE4, 0x10, 0x43, 0x3C, 0x90, 0x8F, 0xB0, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_BusinessFaxNumber = new WindowsDevicePropertyKeys(0x91EFF6F3, 0x2E27, 0x42CA, 0x93, 0x3E, 0x7C, 0x99, 0x9F, 0xBE, 0x31, 0x0B, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_BusinessHomePage = new WindowsDevicePropertyKeys(0x56310920, 0x2491, 0x4919, 0x99, 0xCE, 0xEA, 0xDB, 0x06, 0xFA, 0xFD, 0xB2, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_BusinessTelephone = new WindowsDevicePropertyKeys(0x6A15E5A0, 0x0A1E, 0x4CD7, 0xBB, 0x8C, 0xD2, 0xF1, 0xB0, 0xC9, 0x29, 0xBC, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_CallbackTelephone = new WindowsDevicePropertyKeys(0xBF53D1C3, 0x49E0, 0x4F7F, 0x85, 0x67, 0x5A, 0x82, 0x1D, 0x8A, 0xC5, 0x42, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_CarTelephone = new WindowsDevicePropertyKeys(0x8FDC6DEA, 0xB929, 0x412B, 0xBA, 0x90, 0x39, 0x7A, 0x25, 0x74, 0x65, 0xFE, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_Children = new WindowsDevicePropertyKeys(0xD4729704, 0x8EF1, 0x43EF, 0x90, 0x24, 0x2B, 0xD3, 0x81, 0x18, 0x7F, 0xD5, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_CompanyMainTelephone = new WindowsDevicePropertyKeys(0x8589E481, 0x6040, 0x473D, 0xB1, 0x71, 0x7F, 0xA8, 0x9C, 0x27, 0x08, 0xED, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_Department = new WindowsDevicePropertyKeys(0xFC9F7306, 0xFF8F, 0x4D49, 0x9F, 0xB6, 0x3F, 0xFE, 0x5C, 0x09, 0x51, 0xEC, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_EmailAddress = new WindowsDevicePropertyKeys(0xF8FA7FA3, 0xD12B, 0x4785, 0x8A, 0x4E, 0x69, 0x1A, 0x94, 0xF7, 0xA3, 0xE7, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_EmailAddress2 = new WindowsDevicePropertyKeys(0x38965063, 0xEDC8, 0x4268, 0x84, 0x91, 0xB7, 0x72, 0x31, 0x72, 0xCF, 0x29, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_EmailAddress3 = new WindowsDevicePropertyKeys(0x644D37B4, 0xE1B3, 0x4BAD, 0xB0, 0x99, 0x7E, 0x7C, 0x04, 0x96, 0x6A, 0xCA, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_EmailAddresses = new WindowsDevicePropertyKeys(0x84D8F337, 0x981D, 0x44B3, 0x96, 0x15, 0xC7, 0x59, 0x6D, 0xBA, 0x17, 0xE3, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_EmailName = new WindowsDevicePropertyKeys(0xCC6F4F24, 0x6083, 0x4BD4, 0x87, 0x54, 0x67, 0x4D, 0x0D, 0xE8, 0x7A, 0xB8, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_FileAsName = new WindowsDevicePropertyKeys(0xF1A24AA7, 0x9CA7, 0x40F6, 0x89, 0xEC, 0x97, 0xDE, 0xF9, 0xFF, 0xE8, 0xDB, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_FirstName = new WindowsDevicePropertyKeys(0x14977844, 0x6B49, 0x4AAD, 0xA7, 0x14, 0xA4, 0x51, 0x3B, 0xF6, 0x04, 0x60, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_FullName = new WindowsDevicePropertyKeys(0x635E9051, 0x50A5, 0x4BA2, 0xB9, 0xDB, 0x4E, 0xD0, 0x56, 0xC7, 0x72, 0x96, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_Gender = new WindowsDevicePropertyKeys(0x3C8CEE58, 0xD4F0, 0x4CF9, 0xB7, 0x56, 0x4E, 0x5D, 0x24, 0x44, 0x7B, 0xCD, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_Hobbies = new WindowsDevicePropertyKeys(0x5DC2253F, 0x5E11, 0x4ADF, 0x9C, 0xFE, 0x91, 0x0D, 0xD0, 0x1E, 0x3E, 0x70, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_HomeAddress = new WindowsDevicePropertyKeys(0x98F98354, 0x617A, 0x46B8, 0x85, 0x60, 0x5B, 0x1B, 0x64, 0xBF, 0x1F, 0x89, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_HomeAddressCity = new WindowsDevicePropertyKeys(0x176DC63C, 0x2688, 0x4E89, 0x81, 0x43, 0xA3, 0x47, 0x80, 0x0F, 0x25, 0xE9, 65);
        public static WindowsDevicePropertyKeys PKEY_Contact_HomeAddressCountry = new WindowsDevicePropertyKeys(0x08A65AA1, 0xF4C9, 0x43DD, 0x9D, 0xDF, 0xA3, 0x3D, 0x8E, 0x7E, 0xAD, 0x85, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_HomeAddressPostalCode = new WindowsDevicePropertyKeys(0x8AFCC170, 0x8A46, 0x4B53, 0x9E, 0xEE, 0x90, 0xBA, 0xE7, 0x15, 0x1E, 0x62, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_HomeAddressPostOfficeBox = new WindowsDevicePropertyKeys(0x7B9F6399, 0x0A3F, 0x4B12, 0x89, 0xBD, 0x4A, 0xDC, 0x51, 0xC9, 0x18, 0xAF, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_HomeAddressState = new WindowsDevicePropertyKeys(0xC89A23D0, 0x7D6D, 0x4EB8, 0x87, 0xD4, 0x77, 0x6A, 0x82, 0xD4, 0x93, 0xE5, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_HomeAddressStreet = new WindowsDevicePropertyKeys(0x0ADEF160, 0xDB3F, 0x4308, 0x9A, 0x21, 0x06, 0x23, 0x7B, 0x16, 0xFA, 0x2A, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_HomeFaxNumber = new WindowsDevicePropertyKeys(0x660E04D6, 0x81AB, 0x4977, 0xA0, 0x9F, 0x82, 0x31, 0x31, 0x13, 0xAB, 0x26, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_HomeTelephone = new WindowsDevicePropertyKeys(0x176DC63C, 0x2688, 0x4E89, 0x81, 0x43, 0xA3, 0x47, 0x80, 0x0F, 0x25, 0xE9, 20);
        public static WindowsDevicePropertyKeys PKEY_Contact_IMAddress = new WindowsDevicePropertyKeys(0xD68DBD8A, 0x3374, 0x4B81, 0x99, 0x72, 0x3E, 0xC3, 0x06, 0x82, 0xDB, 0x3D, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_Initials = new WindowsDevicePropertyKeys(0xF3D8F40D, 0x50CB, 0x44A2, 0x97, 0x18, 0x40, 0xCB, 0x91, 0x19, 0x49, 0x5D, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_JA_CompanyNamePhonetic = new WindowsDevicePropertyKeys(0x897B3694, 0xFE9E, 0x43E6, 0x80, 0x66, 0x26, 0x0F, 0x59, 0x0C, 0x01, 0x00, 2);
        public static WindowsDevicePropertyKeys PKEY_Contact_JA_FirstNamePhonetic = new WindowsDevicePropertyKeys(0x897B3694, 0xFE9E, 0x43E6, 0x80, 0x66, 0x26, 0x0F, 0x59, 0x0C, 0x01, 0x00, 3);
        public static WindowsDevicePropertyKeys PKEY_Contact_JA_LastNamePhonetic = new WindowsDevicePropertyKeys(0x897B3694, 0xFE9E, 0x43E6, 0x80, 0x66, 0x26, 0x0F, 0x59, 0x0C, 0x01, 0x00, 4);
        public static WindowsDevicePropertyKeys PKEY_Contact_JobTitle = new WindowsDevicePropertyKeys(0x176DC63C, 0x2688, 0x4E89, 0x81, 0x43, 0xA3, 0x47, 0x80, 0x0F, 0x25, 0xE9, 6);
        public static WindowsDevicePropertyKeys PKEY_Contact_Label = new WindowsDevicePropertyKeys(0x97B0AD89, 0xDF49, 0x49CC, 0x83, 0x4E, 0x66, 0x09, 0x74, 0xFD, 0x75, 0x5B, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_LastName = new WindowsDevicePropertyKeys(0x8F367200, 0xC270, 0x457C, 0xB1, 0xD4, 0xE0, 0x7C, 0x5B, 0xCD, 0x90, 0xC7, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_MailingAddress = new WindowsDevicePropertyKeys(0xC0AC206A, 0x827E, 0x4650, 0x95, 0xAE, 0x77, 0xE2, 0xBB, 0x74, 0xFC, 0xC9, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_MiddleName = new WindowsDevicePropertyKeys(0x176DC63C, 0x2688, 0x4E89, 0x81, 0x43, 0xA3, 0x47, 0x80, 0x0F, 0x25, 0xE9, 71);
        public static WindowsDevicePropertyKeys PKEY_Contact_MobileTelephone = new WindowsDevicePropertyKeys(0x176DC63C, 0x2688, 0x4E89, 0x81, 0x43, 0xA3, 0x47, 0x80, 0x0F, 0x25, 0xE9, 35);
        public static WindowsDevicePropertyKeys PKEY_Contact_NickName = new WindowsDevicePropertyKeys(0x176DC63C, 0x2688, 0x4E89, 0x81, 0x43, 0xA3, 0x47, 0x80, 0x0F, 0x25, 0xE9, 74);
        public static WindowsDevicePropertyKeys PKEY_Contact_OfficeLocation = new WindowsDevicePropertyKeys(0x176DC63C, 0x2688, 0x4E89, 0x81, 0x43, 0xA3, 0x47, 0x80, 0x0F, 0x25, 0xE9, 7);
        public static WindowsDevicePropertyKeys PKEY_Contact_OtherAddress = new WindowsDevicePropertyKeys(0x508161FA, 0x313B, 0x43D5, 0x83, 0xA1, 0xC1, 0xAC, 0xCF, 0x68, 0x62, 0x2C, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_OtherAddressCity = new WindowsDevicePropertyKeys(0x6E682923, 0x7F7B, 0x4F0C, 0xA3, 0x37, 0xCF, 0xCA, 0x29, 0x66, 0x87, 0xBF, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_OtherAddressCountry = new WindowsDevicePropertyKeys(0x8F167568, 0x0AAE, 0x4322, 0x8E, 0xD9, 0x60, 0x55, 0xB7, 0xB0, 0xE3, 0x98, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_OtherAddressPostalCode = new WindowsDevicePropertyKeys(0x95C656C1, 0x2ABF, 0x4148, 0x9E, 0xD3, 0x9E, 0xC6, 0x02, 0xE3, 0xB7, 0xCD, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_OtherAddressPostOfficeBox = new WindowsDevicePropertyKeys(0x8B26EA41, 0x058F, 0x43F6, 0xAE, 0xCC, 0x40, 0x35, 0x68, 0x1C, 0xE9, 0x77, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_OtherAddressState = new WindowsDevicePropertyKeys(0x71B377D6, 0xE570, 0x425F, 0xA1, 0x70, 0x80, 0x9F, 0xAE, 0x73, 0xE5, 0x4E, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_OtherAddressStreet = new WindowsDevicePropertyKeys(0xFF962609, 0xB7D6, 0x4999, 0x86, 0x2D, 0x95, 0x18, 0x0D, 0x52, 0x9A, 0xEA, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_PagerTelephone = new WindowsDevicePropertyKeys(0xD6304E01, 0xF8F5, 0x4F45, 0x8B, 0x15, 0xD0, 0x24, 0xA6, 0x29, 0x67, 0x89, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_PersonalTitle = new WindowsDevicePropertyKeys(0x176DC63C, 0x2688, 0x4E89, 0x81, 0x43, 0xA3, 0x47, 0x80, 0x0F, 0x25, 0xE9, 69);
        public static WindowsDevicePropertyKeys PKEY_Contact_PrimaryAddressCity = new WindowsDevicePropertyKeys(0xC8EA94F0, 0xA9E3, 0x4969, 0xA9, 0x4B, 0x9C, 0x62, 0xA9, 0x53, 0x24, 0xE0, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_PrimaryAddressCountry = new WindowsDevicePropertyKeys(0xE53D799D, 0x0F3F, 0x466E, 0xB2, 0xFF, 0x74, 0x63, 0x4A, 0x3C, 0xB7, 0xA4, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_PrimaryAddressPostalCode = new WindowsDevicePropertyKeys(0x18BBD425, 0xECFD, 0x46EF, 0xB6, 0x12, 0x7B, 0x4A, 0x60, 0x34, 0xED, 0xA0, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_PrimaryAddressPostOfficeBox = new WindowsDevicePropertyKeys(0xDE5EF3C7, 0x46E1, 0x484E, 0x99, 0x99, 0x62, 0xC5, 0x30, 0x83, 0x94, 0xC1, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_PrimaryAddressState = new WindowsDevicePropertyKeys(0xF1176DFE, 0x7138, 0x4640, 0x8B, 0x4C, 0xAE, 0x37, 0x5D, 0xC7, 0x0A, 0x6D, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_PrimaryAddressStreet = new WindowsDevicePropertyKeys(0x63C25B20, 0x96BE, 0x488F, 0x87, 0x88, 0xC0, 0x9C, 0x40, 0x7A, 0xD8, 0x12, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_PrimaryEmailAddress = new WindowsDevicePropertyKeys(0x176DC63C, 0x2688, 0x4E89, 0x81, 0x43, 0xA3, 0x47, 0x80, 0x0F, 0x25, 0xE9, 48);
        public static WindowsDevicePropertyKeys PKEY_Contact_PrimaryTelephone = new WindowsDevicePropertyKeys(0x176DC63C, 0x2688, 0x4E89, 0x81, 0x43, 0xA3, 0x47, 0x80, 0x0F, 0x25, 0xE9, 25);
        public static WindowsDevicePropertyKeys PKEY_Contact_Profession = new WindowsDevicePropertyKeys(0x7268AF55, 0x1CE4, 0x4F6E, 0xA4, 0x1F, 0xB6, 0xE4, 0xEF, 0x10, 0xE4, 0xA9, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_SpouseName = new WindowsDevicePropertyKeys(0x9D2408B6, 0x3167, 0x422B, 0x82, 0xB0, 0xF5, 0x83, 0xB7, 0xA7, 0xCF, 0xE3, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_Suffix = new WindowsDevicePropertyKeys(0x176DC63C, 0x2688, 0x4E89, 0x81, 0x43, 0xA3, 0x47, 0x80, 0x0F, 0x25, 0xE9, 73);
        public static WindowsDevicePropertyKeys PKEY_Contact_TelexNumber = new WindowsDevicePropertyKeys(0xC554493C, 0xC1F7, 0x40C1, 0xA7, 0x6C, 0xEF, 0x8C, 0x06, 0x14, 0x00, 0x3E, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_TTYTDDTelephone = new WindowsDevicePropertyKeys(0xAAF16BAC, 0x2B55, 0x45E6, 0x9F, 0x6D, 0x41, 0x5E, 0xB9, 0x49, 0x10, 0xDF, 100);
        public static WindowsDevicePropertyKeys PKEY_Contact_WebPage = new WindowsDevicePropertyKeys(0xE3E0584C, 0xB788, 0x4A5A, 0xBB, 0x20, 0x7F, 0x5A, 0x44, 0xC9, 0xAC, 0xDD, 18);
        public static WindowsDevicePropertyKeys PKEY_AcquisitionID = new WindowsDevicePropertyKeys(0x65A98875, 0x3C80, 0x40AB, 0xAB, 0xBC, 0xEF, 0xDA, 0xF7, 0x7D, 0xBE, 0xE2, 100);
        public static WindowsDevicePropertyKeys PKEY_ApplicationName = new WindowsDevicePropertyKeys(0xF29F85E0, 0x4FF9, 0x1068, 0xAB, 0x91, 0x08, 0x00, 0x2B, 0x27, 0xB3, 0xD9, 18);
        public static WindowsDevicePropertyKeys PKEY_Author = new WindowsDevicePropertyKeys(0xF29F85E0, 0x4FF9, 0x1068, 0xAB, 0x91, 0x08, 0x00, 0x2B, 0x27, 0xB3, 0xD9, 4);
        public static WindowsDevicePropertyKeys PKEY_Capacity = new WindowsDevicePropertyKeys(0x9B174B35, 0x40FF, 0x11D2, 0xA2, 0x7E, 0x00, 0xC0, 0x4F, 0xC3, 0x08, 0x71, 3);
        public static WindowsDevicePropertyKeys PKEY_Category = new WindowsDevicePropertyKeys(0xD5CDD502, 0x2E9C, 0x101B, 0x93, 0x97, 0x08, 0x00, 0x2B, 0x2C, 0xF9, 0xAE, 2);
        public static WindowsDevicePropertyKeys PKEY_Comment = new WindowsDevicePropertyKeys(0xF29F85E0, 0x4FF9, 0x1068, 0xAB, 0x91, 0x08, 0x00, 0x2B, 0x27, 0xB3, 0xD9, 6);
        public static WindowsDevicePropertyKeys PKEY_Company = new WindowsDevicePropertyKeys(0xD5CDD502, 0x2E9C, 0x101B, 0x93, 0x97, 0x08, 0x00, 0x2B, 0x2C, 0xF9, 0xAE, 15);
        public static WindowsDevicePropertyKeys PKEY_ComputerName = new WindowsDevicePropertyKeys(0x28636AA6, 0x953D, 0x11D2, 0xB5, 0xD6, 0x00, 0xC0, 0x4F, 0xD9, 0x18, 0xD0, 5);
        public static WindowsDevicePropertyKeys PKEY_ContainedItems = new WindowsDevicePropertyKeys(0x28636AA6, 0x953D, 0x11D2, 0xB5, 0xD6, 0x00, 0xC0, 0x4F, 0xD9, 0x18, 0xD0, 29);
        public static WindowsDevicePropertyKeys PKEY_ContentStatus = new WindowsDevicePropertyKeys(0xD5CDD502, 0x2E9C, 0x101B, 0x93, 0x97, 0x08, 0x00, 0x2B, 0x2C, 0xF9, 0xAE, 27);
        public static WindowsDevicePropertyKeys PKEY_ContentType = new WindowsDevicePropertyKeys(0xD5CDD502, 0x2E9C, 0x101B, 0x93, 0x97, 0x08, 0x00, 0x2B, 0x2C, 0xF9, 0xAE, 26);
        public static WindowsDevicePropertyKeys PKEY_Copyright = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 11);
        public static WindowsDevicePropertyKeys PKEY_DateAccessed = new WindowsDevicePropertyKeys(0xB725F130, 0x47EF, 0x101A, 0xA5, 0xF1, 0x02, 0x60, 0x8C, 0x9E, 0xEB, 0xAC, 16);
        public static WindowsDevicePropertyKeys PKEY_DateAcquired = new WindowsDevicePropertyKeys(0x2CBAA8F5, 0xD81F, 0x47CA, 0xB1, 0x7A, 0xF8, 0xD8, 0x22, 0x30, 0x01, 0x31, 100);
        public static WindowsDevicePropertyKeys PKEY_DateArchived = new WindowsDevicePropertyKeys(0x43F8D7B7, 0xA444, 0x4F87, 0x93, 0x83, 0x52, 0x27, 0x1C, 0x9B, 0x91, 0x5C, 100);
        public static WindowsDevicePropertyKeys PKEY_DateCompleted = new WindowsDevicePropertyKeys(0x72FAB781, 0xACDA, 0x43E5, 0xB1, 0x55, 0xB2, 0x43, 0x4F, 0x85, 0xE6, 0x78, 100);
        public static WindowsDevicePropertyKeys PKEY_DateCreated = new WindowsDevicePropertyKeys(0xB725F130, 0x47EF, 0x101A, 0xA5, 0xF1, 0x02, 0x60, 0x8C, 0x9E, 0xEB, 0xAC, 15);
        public static WindowsDevicePropertyKeys PKEY_DateImported = new WindowsDevicePropertyKeys(0x14B81DA1, 0x0135, 0x4D31, 0x96, 0xD9, 0x6C, 0xBF, 0xC9, 0x67, 0x1A, 0x99, 18258);
        public static WindowsDevicePropertyKeys PKEY_DateModified = new WindowsDevicePropertyKeys(0xB725F130, 0x47EF, 0x101A, 0xA5, 0xF1, 0x02, 0x60, 0x8C, 0x9E, 0xEB, 0xAC, 14);
        public static WindowsDevicePropertyKeys PKEY_DueDate = new WindowsDevicePropertyKeys(0x3F8472B5, 0xE0AF, 0x4DB2, 0x80, 0x71, 0xC5, 0x3F, 0xE7, 0x6A, 0xE7, 0xCE, 100);
        public static WindowsDevicePropertyKeys PKEY_EndDate = new WindowsDevicePropertyKeys(0xC75FAA05, 0x96FD, 0x49E7, 0x9C, 0xB4, 0x9F, 0x60, 0x10, 0x82, 0xD5, 0x53, 100);
        public static WindowsDevicePropertyKeys PKEY_FileAllocationSize = new WindowsDevicePropertyKeys(0xB725F130, 0x47EF, 0x101A, 0xA5, 0xF1, 0x02, 0x60, 0x8C, 0x9E, 0xEB, 0xAC, 18);
        public static WindowsDevicePropertyKeys PKEY_FileAttributes = new WindowsDevicePropertyKeys(0xB725F130, 0x47EF, 0x101A, 0xA5, 0xF1, 0x02, 0x60, 0x8C, 0x9E, 0xEB, 0xAC, 13);
        public static WindowsDevicePropertyKeys PKEY_FileCount = new WindowsDevicePropertyKeys(0x28636AA6, 0x953D, 0x11D2, 0xB5, 0xD6, 0x00, 0xC0, 0x4F, 0xD9, 0x18, 0xD0, 12);
        public static WindowsDevicePropertyKeys PKEY_FileDescription = new WindowsDevicePropertyKeys(0x0CEF7D53, 0xFA64, 0x11D1, 0xA2, 0x03, 0x00, 0x00, 0xF8, 0x1F, 0xED, 0xEE, 3);
        public static WindowsDevicePropertyKeys PKEY_FileExtension = new WindowsDevicePropertyKeys(0xE4F10A3C, 0x49E6, 0x405D, 0x82, 0x88, 0xA2, 0x3B, 0xD4, 0xEE, 0xAA, 0x6C, 100);
        public static WindowsDevicePropertyKeys PKEY_FileFRN = new WindowsDevicePropertyKeys(0xB725F130, 0x47EF, 0x101A, 0xA5, 0xF1, 0x02, 0x60, 0x8C, 0x9E, 0xEB, 0xAC, 21);
        public static WindowsDevicePropertyKeys PKEY_FileName = new WindowsDevicePropertyKeys(0x41CF5AE0, 0xF75A, 0x4806, 0xBD, 0x87, 0x59, 0xC7, 0xD9, 0x24, 0x8E, 0xB9, 100);
        public static WindowsDevicePropertyKeys PKEY_FileOwner = new WindowsDevicePropertyKeys(0x9B174B34, 0x40FF, 0x11D2, 0xA2, 0x7E, 0x00, 0xC0, 0x4F, 0xC3, 0x08, 0x71, 4);
        public static WindowsDevicePropertyKeys PKEY_FileVersion = new WindowsDevicePropertyKeys(0x0CEF7D53, 0xFA64, 0x11D1, 0xA2, 0x03, 0x00, 0x00, 0xF8, 0x1F, 0xED, 0xEE, 4);
        public static WindowsDevicePropertyKeys PKEY_FindData = new WindowsDevicePropertyKeys(0x28636AA6, 0x953D, 0x11D2, 0xB5, 0xD6, 0x00, 0xC0, 0x4F, 0xD9, 0x18, 0xD0, 0);
        public static WindowsDevicePropertyKeys PKEY_FlagColor = new WindowsDevicePropertyKeys(0x67DF94DE, 0x0CA7, 0x4D6F, 0xB7, 0x92, 0x05, 0x3A, 0x3E, 0x4F, 0x03, 0xCF, 100);
        public static WindowsDevicePropertyKeys PKEY_FlagColorText = new WindowsDevicePropertyKeys(0x45EAE747, 0x8E2A, 0x40AE, 0x8C, 0xBF, 0xCA, 0x52, 0xAB, 0xA6, 0x15, 0x2A, 100);
        public static WindowsDevicePropertyKeys PKEY_FlagStatus = new WindowsDevicePropertyKeys(0xE3E0584C, 0xB788, 0x4A5A, 0xBB, 0x20, 0x7F, 0x5A, 0x44, 0xC9, 0xAC, 0xDD, 12);
        public static WindowsDevicePropertyKeys PKEY_FlagStatusText = new WindowsDevicePropertyKeys(0xDC54FD2E, 0x189D, 0x4871, 0xAA, 0x01, 0x08, 0xC2, 0xF5, 0x7A, 0x4A, 0xBC, 100);
        public static WindowsDevicePropertyKeys PKEY_FreeSpace = new WindowsDevicePropertyKeys(0x9B174B35, 0x40FF, 0x11D2, 0xA2, 0x7E, 0x00, 0xC0, 0x4F, 0xC3, 0x08, 0x71, 2);
        public static WindowsDevicePropertyKeys PKEY_Identity = new WindowsDevicePropertyKeys(0xA26F4AFC, 0x7346, 0x4299, 0xBE, 0x47, 0xEB, 0x1A, 0xE6, 0x13, 0x13, 0x9F, 100);
        public static WindowsDevicePropertyKeys PKEY_Importance = new WindowsDevicePropertyKeys(0xE3E0584C, 0xB788, 0x4A5A, 0xBB, 0x20, 0x7F, 0x5A, 0x44, 0xC9, 0xAC, 0xDD, 11);
        public static WindowsDevicePropertyKeys PKEY_ImportanceText = new WindowsDevicePropertyKeys(0xA3B29791, 0x7713, 0x4E1D, 0xBB, 0x40, 0x17, 0xDB, 0x85, 0xF0, 0x18, 0x31, 100);
        public static WindowsDevicePropertyKeys PKEY_IsAttachment = new WindowsDevicePropertyKeys(0xF23F425C, 0x71A1, 0x4FA8, 0x92, 0x2F, 0x67, 0x8E, 0xA4, 0xA6, 0x04, 0x08, 100);
        public static WindowsDevicePropertyKeys PKEY_IsDeleted = new WindowsDevicePropertyKeys(0x5CDA5FC8, 0x33EE, 0x4FF3, 0x90, 0x94, 0xAE, 0x7B, 0xD8, 0x86, 0x8C, 0x4D, 100);
        public static WindowsDevicePropertyKeys PKEY_IsFlagged = new WindowsDevicePropertyKeys(0x5DA84765, 0xE3FF, 0x4278, 0x86, 0xB0, 0xA2, 0x79, 0x67, 0xFB, 0xDD, 0x03, 100);
        public static WindowsDevicePropertyKeys PKEY_IsFlaggedComplete = new WindowsDevicePropertyKeys(0xA6F360D2, 0x55F9, 0x48DE, 0xB9, 0x09, 0x62, 0x0E, 0x09, 0x0A, 0x64, 0x7C, 100);
        public static WindowsDevicePropertyKeys PKEY_IsIncomplete = new WindowsDevicePropertyKeys(0x346C8BD1, 0x2E6A, 0x4C45, 0x89, 0xA4, 0x61, 0xB7, 0x8E, 0x8E, 0x70, 0x0F, 100);
        public static WindowsDevicePropertyKeys PKEY_IsRead = new WindowsDevicePropertyKeys(0xE3E0584C, 0xB788, 0x4A5A, 0xBB, 0x20, 0x7F, 0x5A, 0x44, 0xC9, 0xAC, 0xDD, 10);
        public static WindowsDevicePropertyKeys PKEY_IsSendToTarget = new WindowsDevicePropertyKeys(0x28636AA6, 0x953D, 0x11D2, 0xB5, 0xD6, 0x00, 0xC0, 0x4F, 0xD9, 0x18, 0xD0, 33);
        public static WindowsDevicePropertyKeys PKEY_IsShared = new WindowsDevicePropertyKeys(0xEF884C5B, 0x2BFE, 0x41BB, 0xAA, 0xE5, 0x76, 0xEE, 0xDF, 0x4F, 0x99, 0x02, 100);
        public static WindowsDevicePropertyKeys PKEY_ItemAuthors = new WindowsDevicePropertyKeys(0xD0A04F0A, 0x462A, 0x48A4, 0xBB, 0x2F, 0x37, 0x06, 0xE8, 0x8D, 0xBD, 0x7D, 100);
        public static WindowsDevicePropertyKeys PKEY_ItemDate = new WindowsDevicePropertyKeys(0xF7DB74B4, 0x4287, 0x4103, 0xAF, 0xBA, 0xF1, 0xB1, 0x3D, 0xCD, 0x75, 0xCF, 100);
        public static WindowsDevicePropertyKeys PKEY_ItemFolderNameDisplay = new WindowsDevicePropertyKeys(0xB725F130, 0x47EF, 0x101A, 0xA5, 0xF1, 0x02, 0x60, 0x8C, 0x9E, 0xEB, 0xAC, 2);
        public static WindowsDevicePropertyKeys PKEY_ItemFolderPathDisplay = new WindowsDevicePropertyKeys(0xE3E0584C, 0xB788, 0x4A5A, 0xBB, 0x20, 0x7F, 0x5A, 0x44, 0xC9, 0xAC, 0xDD, 6);
        public static WindowsDevicePropertyKeys PKEY_ItemFolderPathDisplayNarrow = new WindowsDevicePropertyKeys(0xDABD30ED, 0x0043, 0x4789, 0xA7, 0xF8, 0xD0, 0x13, 0xA4, 0x73, 0x66, 0x22, 100);
        public static WindowsDevicePropertyKeys PKEY_ItemName = new WindowsDevicePropertyKeys(0x6B8DA074, 0x3B5C, 0x43BC, 0x88, 0x6F, 0x0A, 0x2C, 0xDC, 0xE0, 0x0B, 0x6F, 100);
        public static WindowsDevicePropertyKeys PKEY_ItemNameDisplay = new WindowsDevicePropertyKeys(0xB725F130, 0x47EF, 0x101A, 0xA5, 0xF1, 0x02, 0x60, 0x8C, 0x9E, 0xEB, 0xAC, 10);
        public static WindowsDevicePropertyKeys PKEY_ItemNamePrefix = new WindowsDevicePropertyKeys(0xD7313FF1, 0xA77A, 0x401C, 0x8C, 0x99, 0x3D, 0xBD, 0xD6, 0x8A, 0xDD, 0x36, 100);
        public static WindowsDevicePropertyKeys PKEY_ItemParticipants = new WindowsDevicePropertyKeys(0xD4D0AA16, 0x9948, 0x41A4, 0xAA, 0x85, 0xD9, 0x7F, 0xF9, 0x64, 0x69, 0x93, 100);
        public static WindowsDevicePropertyKeys PKEY_ItemPathDisplay = new WindowsDevicePropertyKeys(0xE3E0584C, 0xB788, 0x4A5A, 0xBB, 0x20, 0x7F, 0x5A, 0x44, 0xC9, 0xAC, 0xDD, 7);
        public static WindowsDevicePropertyKeys PKEY_ItemPathDisplayNarrow = new WindowsDevicePropertyKeys(0x28636AA6, 0x953D, 0x11D2, 0xB5, 0xD6, 0x00, 0xC0, 0x4F, 0xD9, 0x18, 0xD0, 8);
        public static WindowsDevicePropertyKeys PKEY_ItemType = new WindowsDevicePropertyKeys(0x28636AA6, 0x953D, 0x11D2, 0xB5, 0xD6, 0x00, 0xC0, 0x4F, 0xD9, 0x18, 0xD0, 11);
        public static WindowsDevicePropertyKeys PKEY_ItemTypeText = new WindowsDevicePropertyKeys(0xB725F130, 0x47EF, 0x101A, 0xA5, 0xF1, 0x02, 0x60, 0x8C, 0x9E, 0xEB, 0xAC, 4);
        public static WindowsDevicePropertyKeys PKEY_ItemUrl = new WindowsDevicePropertyKeys(0x49691C90, 0x7E17, 0x101A, 0xA9, 0x1C, 0x08, 0x00, 0x2B, 0x2E, 0xCD, 0xA9, 9);
        public static WindowsDevicePropertyKeys PKEY_Keywords = new WindowsDevicePropertyKeys(0xF29F85E0, 0x4FF9, 0x1068, 0xAB, 0x91, 0x08, 0x00, 0x2B, 0x27, 0xB3, 0xD9, 5);
        public static WindowsDevicePropertyKeys PKEY_Kind = new WindowsDevicePropertyKeys(0x1E3EE840, 0xBC2B, 0x476C, 0x82, 0x37, 0x2A, 0xCD, 0x1A, 0x83, 0x9B, 0x22, 3);
        public static WindowsDevicePropertyKeys PKEY_KindText = new WindowsDevicePropertyKeys(0xF04BEF95, 0xC585, 0x4197, 0xA2, 0xB7, 0xDF, 0x46, 0xFD, 0xC9, 0xEE, 0x6D, 100);
        public static WindowsDevicePropertyKeys PKEY_Language = new WindowsDevicePropertyKeys(0xD5CDD502, 0x2E9C, 0x101B, 0x93, 0x97, 0x08, 0x00, 0x2B, 0x2C, 0xF9, 0xAE, 28);
        public static WindowsDevicePropertyKeys PKEY_MileageInformation = new WindowsDevicePropertyKeys(0xFDF84370, 0x031A, 0x4ADD, 0x9E, 0x91, 0x0D, 0x77, 0x5F, 0x1C, 0x66, 0x05, 100);
        public static WindowsDevicePropertyKeys PKEY_MIMEType = new WindowsDevicePropertyKeys(0x0B63E350, 0x9CCC, 0x11D0, 0xBC, 0xDB, 0x00, 0x80, 0x5F, 0xCC, 0xCE, 0x04, 5);
        public static WindowsDevicePropertyKeys PKEY_Null = new WindowsDevicePropertyKeys(0x00000000, 0x0000, 0x0000, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0);
        public static WindowsDevicePropertyKeys PKEY_OfflineAvailability = new WindowsDevicePropertyKeys(0xA94688B6, 0x7D9F, 0x4570, 0xA6, 0x48, 0xE3, 0xDF, 0xC0, 0xAB, 0x2B, 0x3F, 100);
        public static WindowsDevicePropertyKeys PKEY_OfflineStatus = new WindowsDevicePropertyKeys(0x6D24888F, 0x4718, 0x4BDA, 0xAF, 0xED, 0xEA, 0x0F, 0xB4, 0x38, 0x6C, 0xD8, 100);
        public static WindowsDevicePropertyKeys PKEY_OriginalFileName = new WindowsDevicePropertyKeys(0x0CEF7D53, 0xFA64, 0x11D1, 0xA2, 0x03, 0x00, 0x00, 0xF8, 0x1F, 0xED, 0xEE, 6);
        public static WindowsDevicePropertyKeys PKEY_ParentalRating = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 21);
        public static WindowsDevicePropertyKeys PKEY_ParentalRatingReason = new WindowsDevicePropertyKeys(0x10984E0A, 0xF9F2, 0x4321, 0xB7, 0xEF, 0xBA, 0xF1, 0x95, 0xAF, 0x43, 0x19, 100);
        public static WindowsDevicePropertyKeys PKEY_ParentalRatingsOrganization = new WindowsDevicePropertyKeys(0xA7FE0840, 0x1344, 0x46F0, 0x8D, 0x37, 0x52, 0xED, 0x71, 0x2A, 0x4B, 0xF9, 100);
        public static WindowsDevicePropertyKeys PKEY_ParsingBindContext = new WindowsDevicePropertyKeys(0xDFB9A04D, 0x362F, 0x4CA3, 0xB3, 0x0B, 0x02, 0x54, 0xB1, 0x7B, 0x5B, 0x84, 100);
        public static WindowsDevicePropertyKeys PKEY_ParsingName = new WindowsDevicePropertyKeys(0x28636AA6, 0x953D, 0x11D2, 0xB5, 0xD6, 0x00, 0xC0, 0x4F, 0xD9, 0x18, 0xD0, 24);
        public static WindowsDevicePropertyKeys PKEY_ParsingPath = new WindowsDevicePropertyKeys(0x28636AA6, 0x953D, 0x11D2, 0xB5, 0xD6, 0x00, 0xC0, 0x4F, 0xD9, 0x18, 0xD0, 30);
        public static WindowsDevicePropertyKeys PKEY_PerceivedType = new WindowsDevicePropertyKeys(0x28636AA6, 0x953D, 0x11D2, 0xB5, 0xD6, 0x00, 0xC0, 0x4F, 0xD9, 0x18, 0xD0, 9);
        public static WindowsDevicePropertyKeys PKEY_PercentFull = new WindowsDevicePropertyKeys(0x9B174B35, 0x40FF, 0x11D2, 0xA2, 0x7E, 0x00, 0xC0, 0x4F, 0xC3, 0x08, 0x71, 5);
        public static WindowsDevicePropertyKeys PKEY_Priority = new WindowsDevicePropertyKeys(0x9C1FCF74, 0x2D97, 0x41BA, 0xB4, 0xAE, 0xCB, 0x2E, 0x36, 0x61, 0xA6, 0xE4, 5);
        public static WindowsDevicePropertyKeys PKEY_PriorityText = new WindowsDevicePropertyKeys(0xD98BE98B, 0xB86B, 0x4095, 0xBF, 0x52, 0x9D, 0x23, 0xB2, 0xE0, 0xA7, 0x52, 100);
        public static WindowsDevicePropertyKeys PKEY_Project = new WindowsDevicePropertyKeys(0x39A7F922, 0x477C, 0x48DE, 0x8B, 0xC8, 0xB2, 0x84, 0x41, 0xE3, 0x42, 0xE3, 100);
        public static WindowsDevicePropertyKeys PKEY_ProviderItemID = new WindowsDevicePropertyKeys(0xF21D9941, 0x81F0, 0x471A, 0xAD, 0xEE, 0x4E, 0x74, 0xB4, 0x92, 0x17, 0xED, 100);
        public static WindowsDevicePropertyKeys PKEY_Rating = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 9);
        public static WindowsDevicePropertyKeys PKEY_RatingText = new WindowsDevicePropertyKeys(0x90197CA7, 0xFD8F, 0x4E8C, 0x9D, 0xA3, 0xB5, 0x7E, 0x1E, 0x60, 0x92, 0x95, 100);
        public static WindowsDevicePropertyKeys PKEY_Sensitivity = new WindowsDevicePropertyKeys(0xF8D3F6AC, 0x4874, 0x42CB, 0xBE, 0x59, 0xAB, 0x45, 0x4B, 0x30, 0x71, 0x6A, 100);
        public static WindowsDevicePropertyKeys PKEY_SensitivityText = new WindowsDevicePropertyKeys(0xD0C7F054, 0x3F72, 0x4725, 0x85, 0x27, 0x12, 0x9A, 0x57, 0x7C, 0xB2, 0x69, 100);
        public static WindowsDevicePropertyKeys PKEY_SFGAOFlags = new WindowsDevicePropertyKeys(0x28636AA6, 0x953D, 0x11D2, 0xB5, 0xD6, 0x00, 0xC0, 0x4F, 0xD9, 0x18, 0xD0, 25);
        public static WindowsDevicePropertyKeys PKEY_SharedWith = new WindowsDevicePropertyKeys(0xEF884C5B, 0x2BFE, 0x41BB, 0xAA, 0xE5, 0x76, 0xEE, 0xDF, 0x4F, 0x99, 0x02, 200);
        public static WindowsDevicePropertyKeys PKEY_ShareUserRating = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 12);
        public static WindowsDevicePropertyKeys PKEY_Shell_OmitFromView = new WindowsDevicePropertyKeys(0xDE35258C, 0xC695, 0x4CBC, 0xB9, 0x82, 0x38, 0xB0, 0xAD, 0x24, 0xCE, 0xD0, 2);
        public static WindowsDevicePropertyKeys PKEY_SimpleRating = new WindowsDevicePropertyKeys(0xA09F084E, 0xAD41, 0x489F, 0x80, 0x76, 0xAA, 0x5B, 0xE3, 0x08, 0x2B, 0xCA, 100);
        public static WindowsDevicePropertyKeys PKEY_Size = new WindowsDevicePropertyKeys(0xB725F130, 0x47EF, 0x101A, 0xA5, 0xF1, 0x02, 0x60, 0x8C, 0x9E, 0xEB, 0xAC, 12);
        public static WindowsDevicePropertyKeys PKEY_SoftwareUsed = new WindowsDevicePropertyKeys(0x14B81DA1, 0x0135, 0x4D31, 0x96, 0xD9, 0x6C, 0xBF, 0xC9, 0x67, 0x1A, 0x99, 305);
        public static WindowsDevicePropertyKeys PKEY_SourceItem = new WindowsDevicePropertyKeys(0x668CDFA5, 0x7A1B, 0x4323, 0xAE, 0x4B, 0xE5, 0x27, 0x39, 0x3A, 0x1D, 0x81, 100);
        public static WindowsDevicePropertyKeys PKEY_StartDate = new WindowsDevicePropertyKeys(0x48FD6EC8, 0x8A12, 0x4CDF, 0xA0, 0x3E, 0x4E, 0xC5, 0xA5, 0x11, 0xED, 0xDE, 100);
        public static WindowsDevicePropertyKeys PKEY_Status = new WindowsDevicePropertyKeys(0x000214A1, 0x0000, 0x0000, 0xC0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x46, 9);
        public static WindowsDevicePropertyKeys PKEY_Subject = new WindowsDevicePropertyKeys(0xF29F85E0, 0x4FF9, 0x1068, 0xAB, 0x91, 0x08, 0x00, 0x2B, 0x27, 0xB3, 0xD9, 3);
        public static WindowsDevicePropertyKeys PKEY_Thumbnail = new WindowsDevicePropertyKeys(0xF29F85E0, 0x4FF9, 0x1068, 0xAB, 0x91, 0x08, 0x00, 0x2B, 0x27, 0xB3, 0xD9, 17);
        public static WindowsDevicePropertyKeys PKEY_ThumbnailCacheId = new WindowsDevicePropertyKeys(0x446D16B1, 0x8DAD, 0x4870, 0xA7, 0x48, 0x40, 0x2E, 0xA4, 0x3D, 0x78, 0x8C, 100);
        public static WindowsDevicePropertyKeys PKEY_ThumbnailStream = new WindowsDevicePropertyKeys(0xF29F85E0, 0x4FF9, 0x1068, 0xAB, 0x91, 0x08, 0x00, 0x2B, 0x27, 0xB3, 0xD9, 27);
        public static WindowsDevicePropertyKeys PKEY_Title = new WindowsDevicePropertyKeys(0xF29F85E0, 0x4FF9, 0x1068, 0xAB, 0x91, 0x08, 0x00, 0x2B, 0x27, 0xB3, 0xD9, 2);
        public static WindowsDevicePropertyKeys PKEY_TotalFileSize = new WindowsDevicePropertyKeys(0x28636AA6, 0x953D, 0x11D2, 0xB5, 0xD6, 0x00, 0xC0, 0x4F, 0xD9, 0x18, 0xD0, 14);
        public static WindowsDevicePropertyKeys PKEY_Trademarks = new WindowsDevicePropertyKeys(0x0CEF7D53, 0xFA64, 0x11D1, 0xA2, 0x03, 0x00, 0x00, 0xF8, 0x1F, 0xED, 0xEE, 9);
        public static WindowsDevicePropertyKeys PKEY_Document_ByteCount = new WindowsDevicePropertyKeys(0xD5CDD502, 0x2E9C, 0x101B, 0x93, 0x97, 0x08, 0x00, 0x2B, 0x2C, 0xF9, 0xAE, 4);
        public static WindowsDevicePropertyKeys PKEY_Document_CharacterCount = new WindowsDevicePropertyKeys(0xF29F85E0, 0x4FF9, 0x1068, 0xAB, 0x91, 0x08, 0x00, 0x2B, 0x27, 0xB3, 0xD9, 16);
        public static WindowsDevicePropertyKeys PKEY_Document_ClientID = new WindowsDevicePropertyKeys(0x276D7BB0, 0x5B34, 0x4FB0, 0xAA, 0x4B, 0x15, 0x8E, 0xD1, 0x2A, 0x18, 0x09, 100);
        public static WindowsDevicePropertyKeys PKEY_Document_Contributor = new WindowsDevicePropertyKeys(0xF334115E, 0xDA1B, 0x4509, 0x9B, 0x3D, 0x11, 0x95, 0x04, 0xDC, 0x7A, 0xBB, 100);
        public static WindowsDevicePropertyKeys PKEY_Document_DateCreated = new WindowsDevicePropertyKeys(0xF29F85E0, 0x4FF9, 0x1068, 0xAB, 0x91, 0x08, 0x00, 0x2B, 0x27, 0xB3, 0xD9, 12);
        public static WindowsDevicePropertyKeys PKEY_Document_DatePrinted = new WindowsDevicePropertyKeys(0xF29F85E0, 0x4FF9, 0x1068, 0xAB, 0x91, 0x08, 0x00, 0x2B, 0x27, 0xB3, 0xD9, 11);
        public static WindowsDevicePropertyKeys PKEY_Document_DateSaved = new WindowsDevicePropertyKeys(0xF29F85E0, 0x4FF9, 0x1068, 0xAB, 0x91, 0x08, 0x00, 0x2B, 0x27, 0xB3, 0xD9, 13);
        public static WindowsDevicePropertyKeys PKEY_Document_Division = new WindowsDevicePropertyKeys(0x1E005EE6, 0xBF27, 0x428B, 0xB0, 0x1C, 0x79, 0x67, 0x6A, 0xCD, 0x28, 0x70, 100);
        public static WindowsDevicePropertyKeys PKEY_Document_DocumentID = new WindowsDevicePropertyKeys(0xE08805C8, 0xE395, 0x40DF, 0x80, 0xD2, 0x54, 0xF0, 0xD6, 0xC4, 0x31, 0x54, 100);
        public static WindowsDevicePropertyKeys PKEY_Document_HiddenSlideCount = new WindowsDevicePropertyKeys(0xD5CDD502, 0x2E9C, 0x101B, 0x93, 0x97, 0x08, 0x00, 0x2B, 0x2C, 0xF9, 0xAE, 9);
        public static WindowsDevicePropertyKeys PKEY_Document_LastAuthor = new WindowsDevicePropertyKeys(0xF29F85E0, 0x4FF9, 0x1068, 0xAB, 0x91, 0x08, 0x00, 0x2B, 0x27, 0xB3, 0xD9, 8);
        public static WindowsDevicePropertyKeys PKEY_Document_LineCount = new WindowsDevicePropertyKeys(0xD5CDD502, 0x2E9C, 0x101B, 0x93, 0x97, 0x08, 0x00, 0x2B, 0x2C, 0xF9, 0xAE, 5);
        public static WindowsDevicePropertyKeys PKEY_Document_Manager = new WindowsDevicePropertyKeys(0xD5CDD502, 0x2E9C, 0x101B, 0x93, 0x97, 0x08, 0x00, 0x2B, 0x2C, 0xF9, 0xAE, 14);
        public static WindowsDevicePropertyKeys PKEY_Document_MultimediaClipCount = new WindowsDevicePropertyKeys(0xD5CDD502, 0x2E9C, 0x101B, 0x93, 0x97, 0x08, 0x00, 0x2B, 0x2C, 0xF9, 0xAE, 10);
        public static WindowsDevicePropertyKeys PKEY_Document_NoteCount = new WindowsDevicePropertyKeys(0xD5CDD502, 0x2E9C, 0x101B, 0x93, 0x97, 0x08, 0x00, 0x2B, 0x2C, 0xF9, 0xAE, 8);
        public static WindowsDevicePropertyKeys PKEY_Document_PageCount = new WindowsDevicePropertyKeys(0xF29F85E0, 0x4FF9, 0x1068, 0xAB, 0x91, 0x08, 0x00, 0x2B, 0x27, 0xB3, 0xD9, 14);
        public static WindowsDevicePropertyKeys PKEY_Document_ParagraphCount = new WindowsDevicePropertyKeys(0xD5CDD502, 0x2E9C, 0x101B, 0x93, 0x97, 0x08, 0x00, 0x2B, 0x2C, 0xF9, 0xAE, 6);
        public static WindowsDevicePropertyKeys PKEY_Document_PresentationFormat = new WindowsDevicePropertyKeys(0xD5CDD502, 0x2E9C, 0x101B, 0x93, 0x97, 0x08, 0x00, 0x2B, 0x2C, 0xF9, 0xAE, 3);
        public static WindowsDevicePropertyKeys PKEY_Document_RevisionNumber = new WindowsDevicePropertyKeys(0xF29F85E0, 0x4FF9, 0x1068, 0xAB, 0x91, 0x08, 0x00, 0x2B, 0x27, 0xB3, 0xD9, 9);
        public static WindowsDevicePropertyKeys PKEY_Document_Security = new WindowsDevicePropertyKeys(0xF29F85E0, 0x4FF9, 0x1068, 0xAB, 0x91, 0x08, 0x00, 0x2B, 0x27, 0xB3, 0xD9, 19);
        public static WindowsDevicePropertyKeys PKEY_Document_SlideCount = new WindowsDevicePropertyKeys(0xD5CDD502, 0x2E9C, 0x101B, 0x93, 0x97, 0x08, 0x00, 0x2B, 0x2C, 0xF9, 0xAE, 7);
        public static WindowsDevicePropertyKeys PKEY_Document_Template = new WindowsDevicePropertyKeys(0xF29F85E0, 0x4FF9, 0x1068, 0xAB, 0x91, 0x08, 0x00, 0x2B, 0x27, 0xB3, 0xD9, 7);
        public static WindowsDevicePropertyKeys PKEY_Document_TotalEditingTime = new WindowsDevicePropertyKeys(0xF29F85E0, 0x4FF9, 0x1068, 0xAB, 0x91, 0x08, 0x00, 0x2B, 0x27, 0xB3, 0xD9, 10);
        public static WindowsDevicePropertyKeys PKEY_Document_Version = new WindowsDevicePropertyKeys(0xD5CDD502, 0x2E9C, 0x101B, 0x93, 0x97, 0x08, 0x00, 0x2B, 0x2C, 0xF9, 0xAE, 29);
        public static WindowsDevicePropertyKeys PKEY_Document_WordCount = new WindowsDevicePropertyKeys(0xF29F85E0, 0x4FF9, 0x1068, 0xAB, 0x91, 0x08, 0x00, 0x2B, 0x27, 0xB3, 0xD9, 15);
        public static WindowsDevicePropertyKeys PKEY_DRM_DatePlayExpires = new WindowsDevicePropertyKeys(0xAEAC19E4, 0x89AE, 0x4508, 0xB9, 0xB7, 0xBB, 0x86, 0x7A, 0xBE, 0xE2, 0xED, 6);
        public static WindowsDevicePropertyKeys PKEY_DRM_DatePlayStarts = new WindowsDevicePropertyKeys(0xAEAC19E4, 0x89AE, 0x4508, 0xB9, 0xB7, 0xBB, 0x86, 0x7A, 0xBE, 0xE2, 0xED, 5);
        public static WindowsDevicePropertyKeys PKEY_DRM_Description = new WindowsDevicePropertyKeys(0xAEAC19E4, 0x89AE, 0x4508, 0xB9, 0xB7, 0xBB, 0x86, 0x7A, 0xBE, 0xE2, 0xED, 3);
        public static WindowsDevicePropertyKeys PKEY_DRM_IsProtected = new WindowsDevicePropertyKeys(0xAEAC19E4, 0x89AE, 0x4508, 0xB9, 0xB7, 0xBB, 0x86, 0x7A, 0xBE, 0xE2, 0xED, 2);
        public static WindowsDevicePropertyKeys PKEY_DRM_PlayCount = new WindowsDevicePropertyKeys(0xAEAC19E4, 0x89AE, 0x4508, 0xB9, 0xB7, 0xBB, 0x86, 0x7A, 0xBE, 0xE2, 0xED, 4);
        public static WindowsDevicePropertyKeys PKEY_GPS_Altitude = new WindowsDevicePropertyKeys(0x827EDB4F, 0x5B73, 0x44A7, 0x89, 0x1D, 0xFD, 0xFF, 0xAB, 0xEA, 0x35, 0xCA, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_AltitudeDenominator = new WindowsDevicePropertyKeys(0x78342DCB, 0xE358, 0x4145, 0xAE, 0x9A, 0x6B, 0xFE, 0x4E, 0x0F, 0x9F, 0x51, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_AltitudeNumerator = new WindowsDevicePropertyKeys(0x2DAD1EB7, 0x816D, 0x40D3, 0x9E, 0xC3, 0xC9, 0x77, 0x3B, 0xE2, 0xAA, 0xDE, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_AltitudeRef = new WindowsDevicePropertyKeys(0x46AC629D, 0x75EA, 0x4515, 0x86, 0x7F, 0x6D, 0xC4, 0x32, 0x1C, 0x58, 0x44, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_AreaInformation = new WindowsDevicePropertyKeys(0x972E333E, 0xAC7E, 0x49F1, 0x8A, 0xDF, 0xA7, 0x0D, 0x07, 0xA9, 0xBC, 0xAB, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_Date = new WindowsDevicePropertyKeys(0x3602C812, 0x0F3B, 0x45F0, 0x85, 0xAD, 0x60, 0x34, 0x68, 0xD6, 0x94, 0x23, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_DestBearing = new WindowsDevicePropertyKeys(0xC66D4B3C, 0xE888, 0x47CC, 0xB9, 0x9F, 0x9D, 0xCA, 0x3E, 0xE3, 0x4D, 0xEA, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_DestBearingDenominator = new WindowsDevicePropertyKeys(0x7ABCF4F8, 0x7C3F, 0x4988, 0xAC, 0x91, 0x8D, 0x2C, 0x2E, 0x97, 0xEC, 0xA5, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_DestBearingNumerator = new WindowsDevicePropertyKeys(0xBA3B1DA9, 0x86EE, 0x4B5D, 0xA2, 0xA4, 0xA2, 0x71, 0xA4, 0x29, 0xF0, 0xCF, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_DestBearingRef = new WindowsDevicePropertyKeys(0x9AB84393, 0x2A0F, 0x4B75, 0xBB, 0x22, 0x72, 0x79, 0x78, 0x69, 0x77, 0xCB, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_DestDistance = new WindowsDevicePropertyKeys(0xA93EAE04, 0x6804, 0x4F24, 0xAC, 0x81, 0x09, 0xB2, 0x66, 0x45, 0x21, 0x18, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_DestDistanceDenominator = new WindowsDevicePropertyKeys(0x9BC2C99B, 0xAC71, 0x4127, 0x9D, 0x1C, 0x25, 0x96, 0xD0, 0xD7, 0xDC, 0xB7, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_DestDistanceNumerator = new WindowsDevicePropertyKeys(0x2BDA47DA, 0x08C6, 0x4FE1, 0x80, 0xBC, 0xA7, 0x2F, 0xC5, 0x17, 0xC5, 0xD0, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_DestDistanceRef = new WindowsDevicePropertyKeys(0xED4DF2D3, 0x8695, 0x450B, 0x85, 0x6F, 0xF5, 0xC1, 0xC5, 0x3A, 0xCB, 0x66, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_DestLatitude = new WindowsDevicePropertyKeys(0x9D1D7CC5, 0x5C39, 0x451C, 0x86, 0xB3, 0x92, 0x8E, 0x2D, 0x18, 0xCC, 0x47, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_DestLatitudeDenominator = new WindowsDevicePropertyKeys(0x3A372292, 0x7FCA, 0x49A7, 0x99, 0xD5, 0xE4, 0x7B, 0xB2, 0xD4, 0xE7, 0xAB, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_DestLatitudeNumerator = new WindowsDevicePropertyKeys(0xECF4B6F6, 0xD5A6, 0x433C, 0xBB, 0x92, 0x40, 0x76, 0x65, 0x0F, 0xC8, 0x90, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_DestLatitudeRef = new WindowsDevicePropertyKeys(0xCEA820B9, 0xCE61, 0x4885, 0xA1, 0x28, 0x00, 0x5D, 0x90, 0x87, 0xC1, 0x92, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_DestLongitude = new WindowsDevicePropertyKeys(0x47A96261, 0xCB4C, 0x4807, 0x8A, 0xD3, 0x40, 0xB9, 0xD9, 0xDB, 0xC6, 0xBC, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_DestLongitudeDenominator = new WindowsDevicePropertyKeys(0x425D69E5, 0x48AD, 0x4900, 0x8D, 0x80, 0x6E, 0xB6, 0xB8, 0xD0, 0xAC, 0x86, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_DestLongitudeNumerator = new WindowsDevicePropertyKeys(0xA3250282, 0xFB6D, 0x48D5, 0x9A, 0x89, 0xDB, 0xCA, 0xCE, 0x75, 0xCC, 0xCF, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_DestLongitudeRef = new WindowsDevicePropertyKeys(0x182C1EA6, 0x7C1C, 0x4083, 0xAB, 0x4B, 0xAC, 0x6C, 0x9F, 0x4E, 0xD1, 0x28, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_Differential = new WindowsDevicePropertyKeys(0xAAF4EE25, 0xBD3B, 0x4DD7, 0xBF, 0xC4, 0x47, 0xF7, 0x7B, 0xB0, 0x0F, 0x6D, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_DOP = new WindowsDevicePropertyKeys(0x0CF8FB02, 0x1837, 0x42F1, 0xA6, 0x97, 0xA7, 0x01, 0x7A, 0xA2, 0x89, 0xB9, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_DOPDenominator = new WindowsDevicePropertyKeys(0xA0BE94C5, 0x50BA, 0x487B, 0xBD, 0x35, 0x06, 0x54, 0xBE, 0x88, 0x81, 0xED, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_DOPNumerator = new WindowsDevicePropertyKeys(0x47166B16, 0x364F, 0x4AA0, 0x9F, 0x31, 0xE2, 0xAB, 0x3D, 0xF4, 0x49, 0xC3, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_ImgDirection = new WindowsDevicePropertyKeys(0x16473C91, 0xD017, 0x4ED9, 0xBA, 0x4D, 0xB6, 0xBA, 0xA5, 0x5D, 0xBC, 0xF8, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_ImgDirectionDenominator = new WindowsDevicePropertyKeys(0x10B24595, 0x41A2, 0x4E20, 0x93, 0xC2, 0x57, 0x61, 0xC1, 0x39, 0x5F, 0x32, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_ImgDirectionNumerator = new WindowsDevicePropertyKeys(0xDC5877C7, 0x225F, 0x45F7, 0xBA, 0xC7, 0xE8, 0x13, 0x34, 0xB6, 0x13, 0x0A, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_ImgDirectionRef = new WindowsDevicePropertyKeys(0xA4AAA5B7, 0x1AD0, 0x445F, 0x81, 0x1A, 0x0F, 0x8F, 0x6E, 0x67, 0xF6, 0xB5, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_Latitude = new WindowsDevicePropertyKeys(0x8727CFFF, 0x4868, 0x4EC6, 0xAD, 0x5B, 0x81, 0xB9, 0x85, 0x21, 0xD1, 0xAB, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_LatitudeDenominator = new WindowsDevicePropertyKeys(0x16E634EE, 0x2BFF, 0x497B, 0xBD, 0x8A, 0x43, 0x41, 0xAD, 0x39, 0xEE, 0xB9, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_LatitudeNumerator = new WindowsDevicePropertyKeys(0x7DDAAAD1, 0xCCC8, 0x41AE, 0xB7, 0x50, 0xB2, 0xCB, 0x80, 0x31, 0xAE, 0xA2, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_LatitudeRef = new WindowsDevicePropertyKeys(0x029C0252, 0x5B86, 0x46C7, 0xAC, 0xA0, 0x27, 0x69, 0xFF, 0xC8, 0xE3, 0xD4, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_Longitude = new WindowsDevicePropertyKeys(0xC4C4DBB2, 0xB593, 0x466B, 0xBB, 0xDA, 0xD0, 0x3D, 0x27, 0xD5, 0xE4, 0x3A, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_LongitudeDenominator = new WindowsDevicePropertyKeys(0xBE6E176C, 0x4534, 0x4D2C, 0xAC, 0xE5, 0x31, 0xDE, 0xDA, 0xC1, 0x60, 0x6B, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_LongitudeNumerator = new WindowsDevicePropertyKeys(0x02B0F689, 0xA914, 0x4E45, 0x82, 0x1D, 0x1D, 0xDA, 0x45, 0x2E, 0xD2, 0xC4, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_LongitudeRef = new WindowsDevicePropertyKeys(0x33DCF22B, 0x28D5, 0x464C, 0x80, 0x35, 0x1E, 0xE9, 0xEF, 0xD2, 0x52, 0x78, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_MapDatum = new WindowsDevicePropertyKeys(0x2CA2DAE6, 0xEDDC, 0x407D, 0xBE, 0xF1, 0x77, 0x39, 0x42, 0xAB, 0xFA, 0x95, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_MeasureMode = new WindowsDevicePropertyKeys(0xA015ED5D, 0xAAEA, 0x4D58, 0x8A, 0x86, 0x3C, 0x58, 0x69, 0x20, 0xEA, 0x0B, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_ProcessingMethod = new WindowsDevicePropertyKeys(0x59D49E61, 0x840F, 0x4AA9, 0xA9, 0x39, 0xE2, 0x09, 0x9B, 0x7F, 0x63, 0x99, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_Satellites = new WindowsDevicePropertyKeys(0x467EE575, 0x1F25, 0x4557, 0xAD, 0x4E, 0xB8, 0xB5, 0x8B, 0x0D, 0x9C, 0x15, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_Speed = new WindowsDevicePropertyKeys(0xDA5D0862, 0x6E76, 0x4E1B, 0xBA, 0xBD, 0x70, 0x02, 0x1B, 0xD2, 0x54, 0x94, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_SpeedDenominator = new WindowsDevicePropertyKeys(0x7D122D5A, 0xAE5E, 0x4335, 0x88, 0x41, 0xD7, 0x1E, 0x7C, 0xE7, 0x2F, 0x53, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_SpeedNumerator = new WindowsDevicePropertyKeys(0xACC9CE3D, 0xC213, 0x4942, 0x8B, 0x48, 0x6D, 0x08, 0x20, 0xF2, 0x1C, 0x6D, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_SpeedRef = new WindowsDevicePropertyKeys(0xECF7F4C9, 0x544F, 0x4D6D, 0x9D, 0x98, 0x8A, 0xD7, 0x9A, 0xDA, 0xF4, 0x53, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_Status = new WindowsDevicePropertyKeys(0x125491F4, 0x818F, 0x46B2, 0x91, 0xB5, 0xD5, 0x37, 0x75, 0x36, 0x17, 0xB2, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_Track = new WindowsDevicePropertyKeys(0x76C09943, 0x7C33, 0x49E3, 0x9E, 0x7E, 0xCD, 0xBA, 0x87, 0x2C, 0xFA, 0xDA, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_TrackDenominator = new WindowsDevicePropertyKeys(0xC8D1920C, 0x01F6, 0x40C0, 0xAC, 0x86, 0x2F, 0x3A, 0x4A, 0xD0, 0x07, 0x70, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_TrackNumerator = new WindowsDevicePropertyKeys(0x702926F4, 0x44A6, 0x43E1, 0xAE, 0x71, 0x45, 0x62, 0x71, 0x16, 0x89, 0x3B, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_TrackRef = new WindowsDevicePropertyKeys(0x35DBE6FE, 0x44C3, 0x4400, 0xAA, 0xAE, 0xD2, 0xC7, 0x99, 0xC4, 0x07, 0xE8, 100);
        public static WindowsDevicePropertyKeys PKEY_GPS_VersionID = new WindowsDevicePropertyKeys(0x22704DA4, 0xC6B2, 0x4A99, 0x8E, 0x56, 0xF1, 0x6D, 0xF8, 0xC9, 0x25, 0x99, 100);
        public static WindowsDevicePropertyKeys PKEY_Image_BitDepth = new WindowsDevicePropertyKeys(0x6444048F, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 7);
        public static WindowsDevicePropertyKeys PKEY_Image_ColorSpace = new WindowsDevicePropertyKeys(0x14B81DA1, 0x0135, 0x4D31, 0x96, 0xD9, 0x6C, 0xBF, 0xC9, 0x67, 0x1A, 0x99, 40961);
        public static WindowsDevicePropertyKeys PKEY_Image_CompressedBitsPerPixel = new WindowsDevicePropertyKeys(0x364B6FA9, 0x37AB, 0x482A, 0xBE, 0x2B, 0xAE, 0x02, 0xF6, 0x0D, 0x43, 0x18, 100);
        public static WindowsDevicePropertyKeys PKEY_Image_CompressedBitsPerPixelDenominator = new WindowsDevicePropertyKeys(0x1F8844E1, 0x24AD, 0x4508, 0x9D, 0xFD, 0x53, 0x26, 0xA4, 0x15, 0xCE, 0x02, 100);
        public static WindowsDevicePropertyKeys PKEY_Image_CompressedBitsPerPixelNumerator = new WindowsDevicePropertyKeys(0xD21A7148, 0xD32C, 0x4624, 0x89, 0x00, 0x27, 0x72, 0x10, 0xF7, 0x9C, 0x0F, 100);
        public static WindowsDevicePropertyKeys PKEY_Image_Compression = new WindowsDevicePropertyKeys(0x14B81DA1, 0x0135, 0x4D31, 0x96, 0xD9, 0x6C, 0xBF, 0xC9, 0x67, 0x1A, 0x99, 259);
        public static WindowsDevicePropertyKeys PKEY_Image_CompressionText = new WindowsDevicePropertyKeys(0x3F08E66F, 0x2F44, 0x4BB9, 0xA6, 0x82, 0xAC, 0x35, 0xD2, 0x56, 0x23, 0x22, 100);
        public static WindowsDevicePropertyKeys PKEY_Image_Dimensions = new WindowsDevicePropertyKeys(0x6444048F, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 13);
        public static WindowsDevicePropertyKeys PKEY_Image_HorizontalResolution = new WindowsDevicePropertyKeys(0x6444048F, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 5);
        public static WindowsDevicePropertyKeys PKEY_Image_HorizontalSize = new WindowsDevicePropertyKeys(0x6444048F, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 3);
        public static WindowsDevicePropertyKeys PKEY_Image_ImageID = new WindowsDevicePropertyKeys(0x10DABE05, 0x32AA, 0x4C29, 0xBF, 0x1A, 0x63, 0xE2, 0xD2, 0x20, 0x58, 0x7F, 100);
        public static WindowsDevicePropertyKeys PKEY_Image_ResolutionUnit = new WindowsDevicePropertyKeys(0x19B51FA6, 0x1F92, 0x4A5C, 0xAB, 0x48, 0x7D, 0xF0, 0xAB, 0xD6, 0x74, 0x44, 100);
        public static WindowsDevicePropertyKeys PKEY_Image_VerticalResolution = new WindowsDevicePropertyKeys(0x6444048F, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 6);
        public static WindowsDevicePropertyKeys PKEY_Image_VerticalSize = new WindowsDevicePropertyKeys(0x6444048F, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 4);
        public static WindowsDevicePropertyKeys PKEY_Journal_Contacts = new WindowsDevicePropertyKeys(0xDEA7C82C, 0x1D89, 0x4A66, 0x94, 0x27, 0xA4, 0xE3, 0xDE, 0xBA, 0xBC, 0xB1, 100);
        public static WindowsDevicePropertyKeys PKEY_Journal_EntryType = new WindowsDevicePropertyKeys(0x95BEB1FC, 0x326D, 0x4644, 0xB3, 0x96, 0xCD, 0x3E, 0xD9, 0x0E, 0x6D, 0xDF, 100);
        public static WindowsDevicePropertyKeys PKEY_Link_Comment = new WindowsDevicePropertyKeys(0xB9B4B3FC, 0x2B51, 0x4A42, 0xB5, 0xD8, 0x32, 0x41, 0x46, 0xAF, 0xCF, 0x25, 5);
        public static WindowsDevicePropertyKeys PKEY_Link_DateVisited = new WindowsDevicePropertyKeys(0x5CBF2787, 0x48CF, 0x4208, 0xB9, 0x0E, 0xEE, 0x5E, 0x5D, 0x42, 0x02, 0x94, 23);
        public static WindowsDevicePropertyKeys PKEY_Link_Description = new WindowsDevicePropertyKeys(0x5CBF2787, 0x48CF, 0x4208, 0xB9, 0x0E, 0xEE, 0x5E, 0x5D, 0x42, 0x02, 0x94, 21);
        public static WindowsDevicePropertyKeys PKEY_Link_Status = new WindowsDevicePropertyKeys(0xB9B4B3FC, 0x2B51, 0x4A42, 0xB5, 0xD8, 0x32, 0x41, 0x46, 0xAF, 0xCF, 0x25, 3);
        public static WindowsDevicePropertyKeys PKEY_Link_TargetExtension = new WindowsDevicePropertyKeys(0x7A7D76F4, 0xB630, 0x4BD7, 0x95, 0xFF, 0x37, 0xCC, 0x51, 0xA9, 0x75, 0xC9, 2);
        public static WindowsDevicePropertyKeys PKEY_Link_TargetParsingPath = new WindowsDevicePropertyKeys(0xB9B4B3FC, 0x2B51, 0x4A42, 0xB5, 0xD8, 0x32, 0x41, 0x46, 0xAF, 0xCF, 0x25, 2);
        public static WindowsDevicePropertyKeys PKEY_Link_TargetSFGAOFlags = new WindowsDevicePropertyKeys(0xB9B4B3FC, 0x2B51, 0x4A42, 0xB5, 0xD8, 0x32, 0x41, 0x46, 0xAF, 0xCF, 0x25, 8);
        public static WindowsDevicePropertyKeys PKEY_Media_AuthorUrl = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 32);
        public static WindowsDevicePropertyKeys PKEY_Media_AverageLevel = new WindowsDevicePropertyKeys(0x09EDD5B6, 0xB301, 0x43C5, 0x99, 0x90, 0xD0, 0x03, 0x02, 0xEF, 0xFD, 0x46, 100);
        public static WindowsDevicePropertyKeys PKEY_Media_ClassPrimaryID = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 13);
        public static WindowsDevicePropertyKeys PKEY_Media_ClassSecondaryID = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 14);
        public static WindowsDevicePropertyKeys PKEY_Media_CollectionGroupID = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 24);
        public static WindowsDevicePropertyKeys PKEY_Media_CollectionID = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 25);
        public static WindowsDevicePropertyKeys PKEY_Media_ContentDistributor = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 18);
        public static WindowsDevicePropertyKeys PKEY_Media_ContentID = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 26);
        public static WindowsDevicePropertyKeys PKEY_Media_CreatorApplication = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 27);
        public static WindowsDevicePropertyKeys PKEY_Media_CreatorApplicationVersion = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 28);
        public static WindowsDevicePropertyKeys PKEY_Media_DateEncoded = new WindowsDevicePropertyKeys(0x2E4B640D, 0x5019, 0x46D8, 0x88, 0x81, 0x55, 0x41, 0x4C, 0xC5, 0xCA, 0xA0, 100);
        public static WindowsDevicePropertyKeys PKEY_Media_DateReleased = new WindowsDevicePropertyKeys(0xDE41CC29, 0x6971, 0x4290, 0xB4, 0x72, 0xF5, 0x9F, 0x2E, 0x2F, 0x31, 0xE2, 100);
        public static WindowsDevicePropertyKeys PKEY_Media_Duration = new WindowsDevicePropertyKeys(0x64440490, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 3);
        public static WindowsDevicePropertyKeys PKEY_Media_DVDID = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 15);
        public static WindowsDevicePropertyKeys PKEY_Media_EncodedBy = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 36);
        public static WindowsDevicePropertyKeys PKEY_Media_EncodingSettings = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 37);
        public static WindowsDevicePropertyKeys PKEY_Media_FrameCount = new WindowsDevicePropertyKeys(0x6444048F, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 12);
        public static WindowsDevicePropertyKeys PKEY_Media_MCDI = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 16);
        public static WindowsDevicePropertyKeys PKEY_Media_MetadataContentProvider = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 17);
        public static WindowsDevicePropertyKeys PKEY_Media_Producer = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 22);
        public static WindowsDevicePropertyKeys PKEY_Media_PromotionUrl = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 33);
        public static WindowsDevicePropertyKeys PKEY_Media_ProtectionType = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 38);
        public static WindowsDevicePropertyKeys PKEY_Media_ProviderRating = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 39);
        public static WindowsDevicePropertyKeys PKEY_Media_ProviderStyle = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 40);
        public static WindowsDevicePropertyKeys PKEY_Media_Publisher = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 30);
        public static WindowsDevicePropertyKeys PKEY_Media_SubscriptionContentId = new WindowsDevicePropertyKeys(0x9AEBAE7A, 0x9644, 0x487D, 0xA9, 0x2C, 0x65, 0x75, 0x85, 0xED, 0x75, 0x1A, 100);
        public static WindowsDevicePropertyKeys PKEY_Media_SubTitle = new WindowsDevicePropertyKeys(0x56A3372E, 0xCE9C, 0x11D2, 0x9F, 0x0E, 0x00, 0x60, 0x97, 0xC6, 0x86, 0xF6, 38);
        public static WindowsDevicePropertyKeys PKEY_Media_UniqueFileIdentifier = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 35);
        public static WindowsDevicePropertyKeys PKEY_Media_UserNoAutoInfo = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 41);
        public static WindowsDevicePropertyKeys PKEY_Media_UserWebUrl = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 34);
        public static WindowsDevicePropertyKeys PKEY_Media_Writer = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 23);
        public static WindowsDevicePropertyKeys PKEY_Media_Year = new WindowsDevicePropertyKeys(0x56A3372E, 0xCE9C, 0x11D2, 0x9F, 0x0E, 0x00, 0x60, 0x97, 0xC6, 0x86, 0xF6, 5);
        public static WindowsDevicePropertyKeys PKEY_Message_AttachmentContents = new WindowsDevicePropertyKeys(0x3143BF7C, 0x80A8, 0x4854, 0x88, 0x80, 0xE2, 0xE4, 0x01, 0x89, 0xBD, 0xD0, 100);
        public static WindowsDevicePropertyKeys PKEY_Message_AttachmentNames = new WindowsDevicePropertyKeys(0xE3E0584C, 0xB788, 0x4A5A, 0xBB, 0x20, 0x7F, 0x5A, 0x44, 0xC9, 0xAC, 0xDD, 21);
        public static WindowsDevicePropertyKeys PKEY_Message_BccAddress = new WindowsDevicePropertyKeys(0xE3E0584C, 0xB788, 0x4A5A, 0xBB, 0x20, 0x7F, 0x5A, 0x44, 0xC9, 0xAC, 0xDD, 2);
        public static WindowsDevicePropertyKeys PKEY_Message_BccName = new WindowsDevicePropertyKeys(0xE3E0584C, 0xB788, 0x4A5A, 0xBB, 0x20, 0x7F, 0x5A, 0x44, 0xC9, 0xAC, 0xDD, 3);
        public static WindowsDevicePropertyKeys PKEY_Message_CcAddress = new WindowsDevicePropertyKeys(0xE3E0584C, 0xB788, 0x4A5A, 0xBB, 0x20, 0x7F, 0x5A, 0x44, 0xC9, 0xAC, 0xDD, 4);
        public static WindowsDevicePropertyKeys PKEY_Message_CcName = new WindowsDevicePropertyKeys(0xE3E0584C, 0xB788, 0x4A5A, 0xBB, 0x20, 0x7F, 0x5A, 0x44, 0xC9, 0xAC, 0xDD, 5);
        public static WindowsDevicePropertyKeys PKEY_Message_ConversationID = new WindowsDevicePropertyKeys(0xDC8F80BD, 0xAF1E, 0x4289, 0x85, 0xB6, 0x3D, 0xFC, 0x1B, 0x49, 0x39, 0x92, 100);
        public static WindowsDevicePropertyKeys PKEY_Message_ConversationIndex = new WindowsDevicePropertyKeys(0xDC8F80BD, 0xAF1E, 0x4289, 0x85, 0xB6, 0x3D, 0xFC, 0x1B, 0x49, 0x39, 0x92, 101);
        public static WindowsDevicePropertyKeys PKEY_Message_DateReceived = new WindowsDevicePropertyKeys(0xE3E0584C, 0xB788, 0x4A5A, 0xBB, 0x20, 0x7F, 0x5A, 0x44, 0xC9, 0xAC, 0xDD, 20);
        public static WindowsDevicePropertyKeys PKEY_Message_DateSent = new WindowsDevicePropertyKeys(0xE3E0584C, 0xB788, 0x4A5A, 0xBB, 0x20, 0x7F, 0x5A, 0x44, 0xC9, 0xAC, 0xDD, 19);
        public static WindowsDevicePropertyKeys PKEY_Message_FromAddress = new WindowsDevicePropertyKeys(0xE3E0584C, 0xB788, 0x4A5A, 0xBB, 0x20, 0x7F, 0x5A, 0x44, 0xC9, 0xAC, 0xDD, 13);
        public static WindowsDevicePropertyKeys PKEY_Message_FromName = new WindowsDevicePropertyKeys(0xE3E0584C, 0xB788, 0x4A5A, 0xBB, 0x20, 0x7F, 0x5A, 0x44, 0xC9, 0xAC, 0xDD, 14);
        public static WindowsDevicePropertyKeys PKEY_Message_HasAttachments = new WindowsDevicePropertyKeys(0x9C1FCF74, 0x2D97, 0x41BA, 0xB4, 0xAE, 0xCB, 0x2E, 0x36, 0x61, 0xA6, 0xE4, 8);
        public static WindowsDevicePropertyKeys PKEY_Message_IsFwdOrReply = new WindowsDevicePropertyKeys(0x9A9BC088, 0x4F6D, 0x469E, 0x99, 0x19, 0xE7, 0x05, 0x41, 0x20, 0x40, 0xF9, 100);
        public static WindowsDevicePropertyKeys PKEY_Message_MessageClass = new WindowsDevicePropertyKeys(0xCD9ED458, 0x08CE, 0x418F, 0xA7, 0x0E, 0xF9, 0x12, 0xC7, 0xBB, 0x9C, 0x5C, 103);
        public static WindowsDevicePropertyKeys PKEY_Message_SenderAddress = new WindowsDevicePropertyKeys(0x0BE1C8E7, 0x1981, 0x4676, 0xAE, 0x14, 0xFD, 0xD7, 0x8F, 0x05, 0xA6, 0xE7, 100);
        public static WindowsDevicePropertyKeys PKEY_Message_SenderName = new WindowsDevicePropertyKeys(0x0DA41CFA, 0xD224, 0x4A18, 0xAE, 0x2F, 0x59, 0x61, 0x58, 0xDB, 0x4B, 0x3A, 100);
        public static WindowsDevicePropertyKeys PKEY_Message_Store = new WindowsDevicePropertyKeys(0xE3E0584C, 0xB788, 0x4A5A, 0xBB, 0x20, 0x7F, 0x5A, 0x44, 0xC9, 0xAC, 0xDD, 15);
        public static WindowsDevicePropertyKeys PKEY_Message_ToAddress = new WindowsDevicePropertyKeys(0xE3E0584C, 0xB788, 0x4A5A, 0xBB, 0x20, 0x7F, 0x5A, 0x44, 0xC9, 0xAC, 0xDD, 16);
        public static WindowsDevicePropertyKeys PKEY_Message_ToDoTitle = new WindowsDevicePropertyKeys(0xBCCC8A3C, 0x8CEF, 0x42E5, 0x9B, 0x1C, 0xC6, 0x90, 0x79, 0x39, 0x8B, 0xC7, 100);
        public static WindowsDevicePropertyKeys PKEY_Message_ToName = new WindowsDevicePropertyKeys(0xE3E0584C, 0xB788, 0x4A5A, 0xBB, 0x20, 0x7F, 0x5A, 0x44, 0xC9, 0xAC, 0xDD, 17);
        public static WindowsDevicePropertyKeys PKEY_Music_AlbumArtist = new WindowsDevicePropertyKeys(0x56A3372E, 0xCE9C, 0x11D2, 0x9F, 0x0E, 0x00, 0x60, 0x97, 0xC6, 0x86, 0xF6, 13);
        public static WindowsDevicePropertyKeys PKEY_Music_AlbumTitle = new WindowsDevicePropertyKeys(0x56A3372E, 0xCE9C, 0x11D2, 0x9F, 0x0E, 0x00, 0x60, 0x97, 0xC6, 0x86, 0xF6, 4);
        public static WindowsDevicePropertyKeys PKEY_Music_Artist = new WindowsDevicePropertyKeys(0x56A3372E, 0xCE9C, 0x11D2, 0x9F, 0x0E, 0x00, 0x60, 0x97, 0xC6, 0x86, 0xF6, 2);
        public static WindowsDevicePropertyKeys PKEY_Music_BeatsPerMinute = new WindowsDevicePropertyKeys(0x56A3372E, 0xCE9C, 0x11D2, 0x9F, 0x0E, 0x00, 0x60, 0x97, 0xC6, 0x86, 0xF6, 35);
        public static WindowsDevicePropertyKeys PKEY_Music_Composer = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 19);
        public static WindowsDevicePropertyKeys PKEY_Music_Conductor = new WindowsDevicePropertyKeys(0x56A3372E, 0xCE9C, 0x11D2, 0x9F, 0x0E, 0x00, 0x60, 0x97, 0xC6, 0x86, 0xF6, 36);
        public static WindowsDevicePropertyKeys PKEY_Music_ContentGroupDescription = new WindowsDevicePropertyKeys(0x56A3372E, 0xCE9C, 0x11D2, 0x9F, 0x0E, 0x00, 0x60, 0x97, 0xC6, 0x86, 0xF6, 33);
        public static WindowsDevicePropertyKeys PKEY_Music_Genre = new WindowsDevicePropertyKeys(0x56A3372E, 0xCE9C, 0x11D2, 0x9F, 0x0E, 0x00, 0x60, 0x97, 0xC6, 0x86, 0xF6, 11);
        public static WindowsDevicePropertyKeys PKEY_Music_InitialKey = new WindowsDevicePropertyKeys(0x56A3372E, 0xCE9C, 0x11D2, 0x9F, 0x0E, 0x00, 0x60, 0x97, 0xC6, 0x86, 0xF6, 34);
        public static WindowsDevicePropertyKeys PKEY_Music_Lyrics = new WindowsDevicePropertyKeys(0x56A3372E, 0xCE9C, 0x11D2, 0x9F, 0x0E, 0x00, 0x60, 0x97, 0xC6, 0x86, 0xF6, 12);
        public static WindowsDevicePropertyKeys PKEY_Music_Mood = new WindowsDevicePropertyKeys(0x56A3372E, 0xCE9C, 0x11D2, 0x9F, 0x0E, 0x00, 0x60, 0x97, 0xC6, 0x86, 0xF6, 39);
        public static WindowsDevicePropertyKeys PKEY_Music_PartOfSet = new WindowsDevicePropertyKeys(0x56A3372E, 0xCE9C, 0x11D2, 0x9F, 0x0E, 0x00, 0x60, 0x97, 0xC6, 0x86, 0xF6, 37);
        public static WindowsDevicePropertyKeys PKEY_Music_Period = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 31);
        public static WindowsDevicePropertyKeys PKEY_Music_SynchronizedLyrics = new WindowsDevicePropertyKeys(0x6B223B6A, 0x162E, 0x4AA9, 0xB3, 0x9F, 0x05, 0xD6, 0x78, 0xFC, 0x6D, 0x77, 100);
        public static WindowsDevicePropertyKeys PKEY_Music_TrackNumber = new WindowsDevicePropertyKeys(0x56A3372E, 0xCE9C, 0x11D2, 0x9F, 0x0E, 0x00, 0x60, 0x97, 0xC6, 0x86, 0xF6, 7);
        public static WindowsDevicePropertyKeys PKEY_Note_Color = new WindowsDevicePropertyKeys(0x4776CAFA, 0xBCE4, 0x4CB1, 0xA2, 0x3E, 0x26, 0x5E, 0x76, 0xD8, 0xEB, 0x11, 100);
        public static WindowsDevicePropertyKeys PKEY_Note_ColorText = new WindowsDevicePropertyKeys(0x46B4E8DE, 0xCDB2, 0x440D, 0x88, 0x5C, 0x16, 0x58, 0xEB, 0x65, 0xB9, 0x14, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_Aperture = new WindowsDevicePropertyKeys(0x14B81DA1, 0x0135, 0x4D31, 0x96, 0xD9, 0x6C, 0xBF, 0xC9, 0x67, 0x1A, 0x99, 37378);
        public static WindowsDevicePropertyKeys PKEY_Photo_ApertureDenominator = new WindowsDevicePropertyKeys(0xE1A9A38B, 0x6685, 0x46BD, 0x87, 0x5E, 0x57, 0x0D, 0xC7, 0xAD, 0x73, 0x20, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_ApertureNumerator = new WindowsDevicePropertyKeys(0x0337ECEC, 0x39FB, 0x4581, 0xA0, 0xBD, 0x4C, 0x4C, 0xC5, 0x1E, 0x99, 0x14, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_Brightness = new WindowsDevicePropertyKeys(0x1A701BF6, 0x478C, 0x4361, 0x83, 0xAB, 0x37, 0x01, 0xBB, 0x05, 0x3C, 0x58, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_BrightnessDenominator = new WindowsDevicePropertyKeys(0x6EBE6946, 0x2321, 0x440A, 0x90, 0xF0, 0xC0, 0x43, 0xEF, 0xD3, 0x24, 0x76, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_BrightnessNumerator = new WindowsDevicePropertyKeys(0x9E7D118F, 0xB314, 0x45A0, 0x8C, 0xFB, 0xD6, 0x54, 0xB9, 0x17, 0xC9, 0xE9, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_CameraManufacturer = new WindowsDevicePropertyKeys(0x14B81DA1, 0x0135, 0x4D31, 0x96, 0xD9, 0x6C, 0xBF, 0xC9, 0x67, 0x1A, 0x99, 271);
        public static WindowsDevicePropertyKeys PKEY_Photo_CameraModel = new WindowsDevicePropertyKeys(0x14B81DA1, 0x0135, 0x4D31, 0x96, 0xD9, 0x6C, 0xBF, 0xC9, 0x67, 0x1A, 0x99, 272);
        public static WindowsDevicePropertyKeys PKEY_Photo_CameraSerialNumber = new WindowsDevicePropertyKeys(0x14B81DA1, 0x0135, 0x4D31, 0x96, 0xD9, 0x6C, 0xBF, 0xC9, 0x67, 0x1A, 0x99, 273);
        public static WindowsDevicePropertyKeys PKEY_Photo_Contrast = new WindowsDevicePropertyKeys(0x2A785BA9, 0x8D23, 0x4DED, 0x82, 0xE6, 0x60, 0xA3, 0x50, 0xC8, 0x6A, 0x10, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_ContrastText = new WindowsDevicePropertyKeys(0x59DDE9F2, 0x5253, 0x40EA, 0x9A, 0x8B, 0x47, 0x9E, 0x96, 0xC6, 0x24, 0x9A, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_DateTaken = new WindowsDevicePropertyKeys(0x14B81DA1, 0x0135, 0x4D31, 0x96, 0xD9, 0x6C, 0xBF, 0xC9, 0x67, 0x1A, 0x99, 36867);
        public static WindowsDevicePropertyKeys PKEY_Photo_DigitalZoom = new WindowsDevicePropertyKeys(0xF85BF840, 0xA925, 0x4BC2, 0xB0, 0xC4, 0x8E, 0x36, 0xB5, 0x98, 0x67, 0x9E, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_DigitalZoomDenominator = new WindowsDevicePropertyKeys(0x745BAF0E, 0xE5C1, 0x4CFB, 0x8A, 0x1B, 0xD0, 0x31, 0xA0, 0xA5, 0x23, 0x93, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_DigitalZoomNumerator = new WindowsDevicePropertyKeys(0x16CBB924, 0x6500, 0x473B, 0xA5, 0xBE, 0xF1, 0x59, 0x9B, 0xCB, 0xE4, 0x13, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_Event = new WindowsDevicePropertyKeys(0x14B81DA1, 0x0135, 0x4D31, 0x96, 0xD9, 0x6C, 0xBF, 0xC9, 0x67, 0x1A, 0x99, 18248);
        public static WindowsDevicePropertyKeys PKEY_Photo_EXIFVersion = new WindowsDevicePropertyKeys(0xD35F743A, 0xEB2E, 0x47F2, 0xA2, 0x86, 0x84, 0x41, 0x32, 0xCB, 0x14, 0x27, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_ExposureBias = new WindowsDevicePropertyKeys(0x14B81DA1, 0x0135, 0x4D31, 0x96, 0xD9, 0x6C, 0xBF, 0xC9, 0x67, 0x1A, 0x99, 37380);
        public static WindowsDevicePropertyKeys PKEY_Photo_ExposureBiasDenominator = new WindowsDevicePropertyKeys(0xAB205E50, 0x04B7, 0x461C, 0xA1, 0x8C, 0x2F, 0x23, 0x38, 0x36, 0xE6, 0x27, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_ExposureBiasNumerator = new WindowsDevicePropertyKeys(0x738BF284, 0x1D87, 0x420B, 0x92, 0xCF, 0x58, 0x34, 0xBF, 0x6E, 0xF9, 0xED, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_ExposureIndex = new WindowsDevicePropertyKeys(0x967B5AF8, 0x995A, 0x46ED, 0x9E, 0x11, 0x35, 0xB3, 0xC5, 0xB9, 0x78, 0x2D, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_ExposureIndexDenominator = new WindowsDevicePropertyKeys(0x93112F89, 0xC28B, 0x492F, 0x8A, 0x9D, 0x4B, 0xE2, 0x06, 0x2C, 0xEE, 0x8A, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_ExposureIndexNumerator = new WindowsDevicePropertyKeys(0xCDEDCF30, 0x8919, 0x44DF, 0x8F, 0x4C, 0x4E, 0xB2, 0xFF, 0xDB, 0x8D, 0x89, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_ExposureProgram = new WindowsDevicePropertyKeys(0x14B81DA1, 0x0135, 0x4D31, 0x96, 0xD9, 0x6C, 0xBF, 0xC9, 0x67, 0x1A, 0x99, 34850);
        public static WindowsDevicePropertyKeys PKEY_Photo_ExposureProgramText = new WindowsDevicePropertyKeys(0xFEC690B7, 0x5F30, 0x4646, 0xAE, 0x47, 0x4C, 0xAA, 0xFB, 0xA8, 0x84, 0xA3, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_ExposureTime = new WindowsDevicePropertyKeys(0x14B81DA1, 0x0135, 0x4D31, 0x96, 0xD9, 0x6C, 0xBF, 0xC9, 0x67, 0x1A, 0x99, 33434);
        public static WindowsDevicePropertyKeys PKEY_Photo_ExposureTimeDenominator = new WindowsDevicePropertyKeys(0x55E98597, 0xAD16, 0x42E0, 0xB6, 0x24, 0x21, 0x59, 0x9A, 0x19, 0x98, 0x38, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_ExposureTimeNumerator = new WindowsDevicePropertyKeys(0x257E44E2, 0x9031, 0x4323, 0xAC, 0x38, 0x85, 0xC5, 0x52, 0x87, 0x1B, 0x2E, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_Flash = new WindowsDevicePropertyKeys(0x14B81DA1, 0x0135, 0x4D31, 0x96, 0xD9, 0x6C, 0xBF, 0xC9, 0x67, 0x1A, 0x99, 37385);
        public static WindowsDevicePropertyKeys PKEY_Photo_FlashEnergy = new WindowsDevicePropertyKeys(0x14B81DA1, 0x0135, 0x4D31, 0x96, 0xD9, 0x6C, 0xBF, 0xC9, 0x67, 0x1A, 0x99, 41483);
        public static WindowsDevicePropertyKeys PKEY_Photo_FlashEnergyDenominator = new WindowsDevicePropertyKeys(0xD7B61C70, 0x6323, 0x49CD, 0xA5, 0xFC, 0xC8, 0x42, 0x77, 0x16, 0x2C, 0x97, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_FlashEnergyNumerator = new WindowsDevicePropertyKeys(0xFCAD3D3D, 0x0858, 0x400F, 0xAA, 0xA3, 0x2F, 0x66, 0xCC, 0xE2, 0xA6, 0xBC, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_FlashManufacturer = new WindowsDevicePropertyKeys(0xAABAF6C9, 0xE0C5, 0x4719, 0x85, 0x85, 0x57, 0xB1, 0x03, 0xE5, 0x84, 0xFE, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_FlashModel = new WindowsDevicePropertyKeys(0xFE83BB35, 0x4D1A, 0x42E2, 0x91, 0x6B, 0x06, 0xF3, 0xE1, 0xAF, 0x71, 0x9E, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_FlashText = new WindowsDevicePropertyKeys(0x6B8B68F6, 0x200B, 0x47EA, 0x8D, 0x25, 0xD8, 0x05, 0x0F, 0x57, 0x33, 0x9F, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_FNumber = new WindowsDevicePropertyKeys(0x14B81DA1, 0x0135, 0x4D31, 0x96, 0xD9, 0x6C, 0xBF, 0xC9, 0x67, 0x1A, 0x99, 33437);
        public static WindowsDevicePropertyKeys PKEY_Photo_FNumberDenominator = new WindowsDevicePropertyKeys(0xE92A2496, 0x223B, 0x4463, 0xA4, 0xE3, 0x30, 0xEA, 0xBB, 0xA7, 0x9D, 0x80, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_FNumberNumerator = new WindowsDevicePropertyKeys(0x1B97738A, 0xFDFC, 0x462F, 0x9D, 0x93, 0x19, 0x57, 0xE0, 0x8B, 0xE9, 0x0C, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_FocalLength = new WindowsDevicePropertyKeys(0x14B81DA1, 0x0135, 0x4D31, 0x96, 0xD9, 0x6C, 0xBF, 0xC9, 0x67, 0x1A, 0x99, 37386);
        public static WindowsDevicePropertyKeys PKEY_Photo_FocalLengthDenominator = new WindowsDevicePropertyKeys(0x305BC615, 0xDCA1, 0x44A5, 0x9F, 0xD4, 0x10, 0xC0, 0xBA, 0x79, 0x41, 0x2E, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_FocalLengthInFilm = new WindowsDevicePropertyKeys(0xA0E74609, 0xB84D, 0x4F49, 0xB8, 0x60, 0x46, 0x2B, 0xD9, 0x97, 0x1F, 0x98, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_FocalLengthNumerator = new WindowsDevicePropertyKeys(0x776B6B3B, 0x1E3D, 0x4B0C, 0x9A, 0x0E, 0x8F, 0xBA, 0xF2, 0xA8, 0x49, 0x2A, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_FocalPlaneXResolution = new WindowsDevicePropertyKeys(0xCFC08D97, 0xC6F7, 0x4484, 0x89, 0xDD, 0xEB, 0xEF, 0x43, 0x56, 0xFE, 0x76, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_FocalPlaneXResolutionDenominator = new WindowsDevicePropertyKeys(0x0933F3F5, 0x4786, 0x4F46, 0xA8, 0xE8, 0xD6, 0x4D, 0xD3, 0x7F, 0xA5, 0x21, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_FocalPlaneXResolutionNumerator = new WindowsDevicePropertyKeys(0xDCCB10AF, 0xB4E2, 0x4B88, 0x95, 0xF9, 0x03, 0x1B, 0x4D, 0x5A, 0xB4, 0x90, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_FocalPlaneYResolution = new WindowsDevicePropertyKeys(0x4FFFE4D0, 0x914F, 0x4AC4, 0x8D, 0x6F, 0xC9, 0xC6, 0x1D, 0xE1, 0x69, 0xB1, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_FocalPlaneYResolutionDenominator = new WindowsDevicePropertyKeys(0x1D6179A6, 0xA876, 0x4031, 0xB0, 0x13, 0x33, 0x47, 0xB2, 0xB6, 0x4D, 0xC8, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_FocalPlaneYResolutionNumerator = new WindowsDevicePropertyKeys(0xA2E541C5, 0x4440, 0x4BA8, 0x86, 0x7E, 0x75, 0xCF, 0xC0, 0x68, 0x28, 0xCD, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_GainControl = new WindowsDevicePropertyKeys(0xFA304789, 0x00C7, 0x4D80, 0x90, 0x4A, 0x1E, 0x4D, 0xCC, 0x72, 0x65, 0xAA, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_GainControlDenominator = new WindowsDevicePropertyKeys(0x42864DFD, 0x9DA4, 0x4F77, 0xBD, 0xED, 0x4A, 0xAD, 0x7B, 0x25, 0x67, 0x35, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_GainControlNumerator = new WindowsDevicePropertyKeys(0x8E8ECF7C, 0xB7B8, 0x4EB8, 0xA6, 0x3F, 0x0E, 0xE7, 0x15, 0xC9, 0x6F, 0x9E, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_GainControlText = new WindowsDevicePropertyKeys(0xC06238B2, 0x0BF9, 0x4279, 0xA7, 0x23, 0x25, 0x85, 0x67, 0x15, 0xCB, 0x9D, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_ISOSpeed = new WindowsDevicePropertyKeys(0x14B81DA1, 0x0135, 0x4D31, 0x96, 0xD9, 0x6C, 0xBF, 0xC9, 0x67, 0x1A, 0x99, 34855);
        public static WindowsDevicePropertyKeys PKEY_Photo_LensManufacturer = new WindowsDevicePropertyKeys(0xE6DDCAF7, 0x29C5, 0x4F0A, 0x9A, 0x68, 0xD1, 0x94, 0x12, 0xEC, 0x70, 0x90, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_LensModel = new WindowsDevicePropertyKeys(0xE1277516, 0x2B5F, 0x4869, 0x89, 0xB1, 0x2E, 0x58, 0x5B, 0xD3, 0x8B, 0x7A, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_LightSource = new WindowsDevicePropertyKeys(0x14B81DA1, 0x0135, 0x4D31, 0x96, 0xD9, 0x6C, 0xBF, 0xC9, 0x67, 0x1A, 0x99, 37384);
        public static WindowsDevicePropertyKeys PKEY_Photo_MakerNote = new WindowsDevicePropertyKeys(0xFA303353, 0xB659, 0x4052, 0x85, 0xE9, 0xBC, 0xAC, 0x79, 0x54, 0x9B, 0x84, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_MakerNoteOffset = new WindowsDevicePropertyKeys(0x813F4124, 0x34E6, 0x4D17, 0xAB, 0x3E, 0x6B, 0x1F, 0x3C, 0x22, 0x47, 0xA1, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_MaxAperture = new WindowsDevicePropertyKeys(0x08F6D7C2, 0xE3F2, 0x44FC, 0xAF, 0x1E, 0x5A, 0xA5, 0xC8, 0x1A, 0x2D, 0x3E, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_MaxApertureDenominator = new WindowsDevicePropertyKeys(0xC77724D4, 0x601F, 0x46C5, 0x9B, 0x89, 0xC5, 0x3F, 0x93, 0xBC, 0xEB, 0x77, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_MaxApertureNumerator = new WindowsDevicePropertyKeys(0xC107E191, 0xA459, 0x44C5, 0x9A, 0xE6, 0xB9, 0x52, 0xAD, 0x4B, 0x90, 0x6D, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_MeteringMode = new WindowsDevicePropertyKeys(0x14B81DA1, 0x0135, 0x4D31, 0x96, 0xD9, 0x6C, 0xBF, 0xC9, 0x67, 0x1A, 0x99, 37383);
        public static WindowsDevicePropertyKeys PKEY_Photo_MeteringModeText = new WindowsDevicePropertyKeys(0xF628FD8C, 0x7BA8, 0x465A, 0xA6, 0x5B, 0xC5, 0xAA, 0x79, 0x26, 0x3A, 0x9E, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_Orientation = new WindowsDevicePropertyKeys(0x14B81DA1, 0x0135, 0x4D31, 0x96, 0xD9, 0x6C, 0xBF, 0xC9, 0x67, 0x1A, 0x99, 274);
        public static WindowsDevicePropertyKeys PKEY_Photo_OrientationText = new WindowsDevicePropertyKeys(0xA9EA193C, 0xC511, 0x498A, 0xA0, 0x6B, 0x58, 0xE2, 0x77, 0x6D, 0xCC, 0x28, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_PhotometricInterpretation = new WindowsDevicePropertyKeys(0x341796F1, 0x1DF9, 0x4B1C, 0xA5, 0x64, 0x91, 0xBD, 0xEF, 0xA4, 0x38, 0x77, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_PhotometricInterpretationText = new WindowsDevicePropertyKeys(0x821437D6, 0x9EAB, 0x4765, 0xA5, 0x89, 0x3B, 0x1C, 0xBB, 0xD2, 0x2A, 0x61, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_ProgramMode = new WindowsDevicePropertyKeys(0x6D217F6D, 0x3F6A, 0x4825, 0xB4, 0x70, 0x5F, 0x03, 0xCA, 0x2F, 0xBE, 0x9B, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_ProgramModeText = new WindowsDevicePropertyKeys(0x7FE3AA27, 0x2648, 0x42F3, 0x89, 0xB0, 0x45, 0x4E, 0x5C, 0xB1, 0x50, 0xC3, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_RelatedSoundFile = new WindowsDevicePropertyKeys(0x318A6B45, 0x087F, 0x4DC2, 0xB8, 0xCC, 0x05, 0x35, 0x95, 0x51, 0xFC, 0x9E, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_Saturation = new WindowsDevicePropertyKeys(0x49237325, 0xA95A, 0x4F67, 0xB2, 0x11, 0x81, 0x6B, 0x2D, 0x45, 0xD2, 0xE0, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_SaturationText = new WindowsDevicePropertyKeys(0x61478C08, 0xB600, 0x4A84, 0xBB, 0xE4, 0xE9, 0x9C, 0x45, 0xF0, 0xA0, 0x72, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_Sharpness = new WindowsDevicePropertyKeys(0xFC6976DB, 0x8349, 0x4970, 0xAE, 0x97, 0xB3, 0xC5, 0x31, 0x6A, 0x08, 0xF0, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_SharpnessText = new WindowsDevicePropertyKeys(0x51EC3F47, 0xDD50, 0x421D, 0x87, 0x69, 0x33, 0x4F, 0x50, 0x42, 0x4B, 0x1E, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_ShutterSpeed = new WindowsDevicePropertyKeys(0x14B81DA1, 0x0135, 0x4D31, 0x96, 0xD9, 0x6C, 0xBF, 0xC9, 0x67, 0x1A, 0x99, 37377);
        public static WindowsDevicePropertyKeys PKEY_Photo_ShutterSpeedDenominator = new WindowsDevicePropertyKeys(0xE13D8975, 0x81C7, 0x4948, 0xAE, 0x3F, 0x37, 0xCA, 0xE1, 0x1E, 0x8F, 0xF7, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_ShutterSpeedNumerator = new WindowsDevicePropertyKeys(0x16EA4042, 0xD6F4, 0x4BCA, 0x83, 0x49, 0x7C, 0x78, 0xD3, 0x0F, 0xB3, 0x33, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_SubjectDistance = new WindowsDevicePropertyKeys(0x14B81DA1, 0x0135, 0x4D31, 0x96, 0xD9, 0x6C, 0xBF, 0xC9, 0x67, 0x1A, 0x99, 37382);
        public static WindowsDevicePropertyKeys PKEY_Photo_SubjectDistanceDenominator = new WindowsDevicePropertyKeys(0x0C840A88, 0xB043, 0x466D, 0x97, 0x66, 0xD4, 0xB2, 0x6D, 0xA3, 0xFA, 0x77, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_SubjectDistanceNumerator = new WindowsDevicePropertyKeys(0x8AF4961C, 0xF526, 0x43E5, 0xAA, 0x81, 0xDB, 0x76, 0x82, 0x19, 0x17, 0x8D, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_TranscodedForSync = new WindowsDevicePropertyKeys(0x9A8EBB75, 0x6458, 0x4E82, 0xBA, 0xCB, 0x35, 0xC0, 0x09, 0x5B, 0x03, 0xBB, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_WhiteBalance = new WindowsDevicePropertyKeys(0xEE3D3D8A, 0x5381, 0x4CFA, 0xB1, 0x3B, 0xAA, 0xF6, 0x6B, 0x5F, 0x4E, 0xC9, 100);
        public static WindowsDevicePropertyKeys PKEY_Photo_WhiteBalanceText = new WindowsDevicePropertyKeys(0x6336B95E, 0xC7A7, 0x426D, 0x86, 0xFD, 0x7A, 0xE3, 0xD3, 0x9C, 0x84, 0xB4, 100);
        public static WindowsDevicePropertyKeys PKEY_PropGroup_Advanced = new WindowsDevicePropertyKeys(0x900A403B, 0x097B, 0x4B95, 0x8A, 0xE2, 0x07, 0x1F, 0xDA, 0xEE, 0xB1, 0x18, 100);
        public static WindowsDevicePropertyKeys PKEY_PropGroup_Audio = new WindowsDevicePropertyKeys(0x2804D469, 0x788F, 0x48AA, 0x85, 0x70, 0x71, 0xB9, 0xC1, 0x87, 0xE1, 0x38, 100);
        public static WindowsDevicePropertyKeys PKEY_PropGroup_Calendar = new WindowsDevicePropertyKeys(0x9973D2B5, 0xBFD8, 0x438A, 0xBA, 0x94, 0x53, 0x49, 0xB2, 0x93, 0x18, 0x1A, 100);
        public static WindowsDevicePropertyKeys PKEY_PropGroup_Camera = new WindowsDevicePropertyKeys(0xDE00DE32, 0x547E, 0x4981, 0xAD, 0x4B, 0x54, 0x2F, 0x2E, 0x90, 0x07, 0xD8, 100);
        public static WindowsDevicePropertyKeys PKEY_PropGroup_Contact = new WindowsDevicePropertyKeys(0xDF975FD3, 0x250A, 0x4004, 0x85, 0x8F, 0x34, 0xE2, 0x9A, 0x3E, 0x37, 0xAA, 100);
        public static WindowsDevicePropertyKeys PKEY_PropGroup_Content = new WindowsDevicePropertyKeys(0xD0DAB0BA, 0x368A, 0x4050, 0xA8, 0x82, 0x6C, 0x01, 0x0F, 0xD1, 0x9A, 0x4F, 100);
        public static WindowsDevicePropertyKeys PKEY_PropGroup_Description = new WindowsDevicePropertyKeys(0x8969B275, 0x9475, 0x4E00, 0xA8, 0x87, 0xFF, 0x93, 0xB8, 0xB4, 0x1E, 0x44, 100);
        public static WindowsDevicePropertyKeys PKEY_PropGroup_FileSystem = new WindowsDevicePropertyKeys(0xE3A7D2C1, 0x80FC, 0x4B40, 0x8F, 0x34, 0x30, 0xEA, 0x11, 0x1B, 0xDC, 0x2E, 100);
        public static WindowsDevicePropertyKeys PKEY_PropGroup_General = new WindowsDevicePropertyKeys(0xCC301630, 0xB192, 0x4C22, 0xB3, 0x72, 0x9F, 0x4C, 0x6D, 0x33, 0x8E, 0x07, 100);
        public static WindowsDevicePropertyKeys PKEY_PropGroup_GPS = new WindowsDevicePropertyKeys(0xF3713ADA, 0x90E3, 0x4E11, 0xAA, 0xE5, 0xFD, 0xC1, 0x76, 0x85, 0xB9, 0xBE, 100);
        public static WindowsDevicePropertyKeys PKEY_PropGroup_Image = new WindowsDevicePropertyKeys(0xE3690A87, 0x0FA8, 0x4A2A, 0x9A, 0x9F, 0xFC, 0xE8, 0x82, 0x70, 0x55, 0xAC, 100);
        public static WindowsDevicePropertyKeys PKEY_PropGroup_Media = new WindowsDevicePropertyKeys(0x61872CF7, 0x6B5E, 0x4B4B, 0xAC, 0x2D, 0x59, 0xDA, 0x84, 0x45, 0x92, 0x48, 100);
        public static WindowsDevicePropertyKeys PKEY_PropGroup_MediaAdvanced = new WindowsDevicePropertyKeys(0x8859A284, 0xDE7E, 0x4642, 0x99, 0xBA, 0xD4, 0x31, 0xD0, 0x44, 0xB1, 0xEC, 100);
        public static WindowsDevicePropertyKeys PKEY_PropGroup_Message = new WindowsDevicePropertyKeys(0x7FD7259D, 0x16B4, 0x4135, 0x9F, 0x97, 0x7C, 0x96, 0xEC, 0xD2, 0xFA, 0x9E, 100);
        public static WindowsDevicePropertyKeys PKEY_PropGroup_Music = new WindowsDevicePropertyKeys(0x68DD6094, 0x7216, 0x40F1, 0xA0, 0x29, 0x43, 0xFE, 0x71, 0x27, 0x04, 0x3F, 100);
        public static WindowsDevicePropertyKeys PKEY_PropGroup_Origin = new WindowsDevicePropertyKeys(0x2598D2FB, 0x5569, 0x4367, 0x95, 0xDF, 0x5C, 0xD3, 0xA1, 0x77, 0xE1, 0xA5, 100);
        public static WindowsDevicePropertyKeys PKEY_PropGroup_PhotoAdvanced = new WindowsDevicePropertyKeys(0x0CB2BF5A, 0x9EE7, 0x4A86, 0x82, 0x22, 0xF0, 0x1E, 0x07, 0xFD, 0xAD, 0xAF, 100);
        public static WindowsDevicePropertyKeys PKEY_PropGroup_RecordedTV = new WindowsDevicePropertyKeys(0xE7B33238, 0x6584, 0x4170, 0xA5, 0xC0, 0xAC, 0x25, 0xEF, 0xD9, 0xDA, 0x56, 100);
        public static WindowsDevicePropertyKeys PKEY_PropGroup_Video = new WindowsDevicePropertyKeys(0xBEBE0920, 0x7671, 0x4C54, 0xA3, 0xEB, 0x49, 0xFD, 0xDF, 0xC1, 0x91, 0xEE, 100);
        public static WindowsDevicePropertyKeys PKEY_PropList_ConflictPrompt = new WindowsDevicePropertyKeys(0xC9944A21, 0xA406, 0x48FE, 0x82, 0x25, 0xAE, 0xC7, 0xE2, 0x4C, 0x21, 0x1B, 11);
        public static WindowsDevicePropertyKeys PKEY_PropList_ExtendedTileInfo = new WindowsDevicePropertyKeys(0xC9944A21, 0xA406, 0x48FE, 0x82, 0x25, 0xAE, 0xC7, 0xE2, 0x4C, 0x21, 0x1B, 9);
        public static WindowsDevicePropertyKeys PKEY_PropList_FileOperationPrompt = new WindowsDevicePropertyKeys(0xC9944A21, 0xA406, 0x48FE, 0x82, 0x25, 0xAE, 0xC7, 0xE2, 0x4C, 0x21, 0x1B, 10);
        public static WindowsDevicePropertyKeys PKEY_PropList_FullDetails = new WindowsDevicePropertyKeys(0xC9944A21, 0xA406, 0x48FE, 0x82, 0x25, 0xAE, 0xC7, 0xE2, 0x4C, 0x21, 0x1B, 2);
        public static WindowsDevicePropertyKeys PKEY_PropList_InfoTip = new WindowsDevicePropertyKeys(0xC9944A21, 0xA406, 0x48FE, 0x82, 0x25, 0xAE, 0xC7, 0xE2, 0x4C, 0x21, 0x1B, 4);
        public static WindowsDevicePropertyKeys PKEY_PropList_NonPersonal = new WindowsDevicePropertyKeys(0x49D1091F, 0x082E, 0x493F, 0xB2, 0x3F, 0xD2, 0x30, 0x8A, 0xA9, 0x66, 0x8C, 100);
        public static WindowsDevicePropertyKeys PKEY_PropList_PreviewDetails = new WindowsDevicePropertyKeys(0xC9944A21, 0xA406, 0x48FE, 0x82, 0x25, 0xAE, 0xC7, 0xE2, 0x4C, 0x21, 0x1B, 8);
        public static WindowsDevicePropertyKeys PKEY_PropList_PreviewTitle = new WindowsDevicePropertyKeys(0xC9944A21, 0xA406, 0x48FE, 0x82, 0x25, 0xAE, 0xC7, 0xE2, 0x4C, 0x21, 0x1B, 6);
        public static WindowsDevicePropertyKeys PKEY_PropList_QuickTip = new WindowsDevicePropertyKeys(0xC9944A21, 0xA406, 0x48FE, 0x82, 0x25, 0xAE, 0xC7, 0xE2, 0x4C, 0x21, 0x1B, 5);
        public static WindowsDevicePropertyKeys PKEY_PropList_TileInfo = new WindowsDevicePropertyKeys(0xC9944A21, 0xA406, 0x48FE, 0x82, 0x25, 0xAE, 0xC7, 0xE2, 0x4C, 0x21, 0x1B, 3);
        public static WindowsDevicePropertyKeys PKEY_PropList_XPDetailsPanel = new WindowsDevicePropertyKeys(0xF2275480, 0xF782, 0x4291, 0xBD, 0x94, 0xF1, 0x36, 0x93, 0x51, 0x3A, 0xEC, 0);
        public static WindowsDevicePropertyKeys PKEY_RecordedTV_ChannelNumber = new WindowsDevicePropertyKeys(0x6D748DE2, 0x8D38, 0x4CC3, 0xAC, 0x60, 0xF0, 0x09, 0xB0, 0x57, 0xC5, 0x57, 7);
        public static WindowsDevicePropertyKeys PKEY_RecordedTV_Credits = new WindowsDevicePropertyKeys(0x6D748DE2, 0x8D38, 0x4CC3, 0xAC, 0x60, 0xF0, 0x09, 0xB0, 0x57, 0xC5, 0x57, 4);
        public static WindowsDevicePropertyKeys PKEY_RecordedTV_DateContentExpires = new WindowsDevicePropertyKeys(0x6D748DE2, 0x8D38, 0x4CC3, 0xAC, 0x60, 0xF0, 0x09, 0xB0, 0x57, 0xC5, 0x57, 15);
        public static WindowsDevicePropertyKeys PKEY_RecordedTV_EpisodeName = new WindowsDevicePropertyKeys(0x6D748DE2, 0x8D38, 0x4CC3, 0xAC, 0x60, 0xF0, 0x09, 0xB0, 0x57, 0xC5, 0x57, 2);
        public static WindowsDevicePropertyKeys PKEY_RecordedTV_IsATSCContent = new WindowsDevicePropertyKeys(0x6D748DE2, 0x8D38, 0x4CC3, 0xAC, 0x60, 0xF0, 0x09, 0xB0, 0x57, 0xC5, 0x57, 16);
        public static WindowsDevicePropertyKeys PKEY_RecordedTV_IsClosedCaptioningAvailable = new WindowsDevicePropertyKeys(0x6D748DE2, 0x8D38, 0x4CC3, 0xAC, 0x60, 0xF0, 0x09, 0xB0, 0x57, 0xC5, 0x57, 12);
        public static WindowsDevicePropertyKeys PKEY_RecordedTV_IsDTVContent = new WindowsDevicePropertyKeys(0x6D748DE2, 0x8D38, 0x4CC3, 0xAC, 0x60, 0xF0, 0x09, 0xB0, 0x57, 0xC5, 0x57, 17);
        public static WindowsDevicePropertyKeys PKEY_RecordedTV_IsHDContent = new WindowsDevicePropertyKeys(0x6D748DE2, 0x8D38, 0x4CC3, 0xAC, 0x60, 0xF0, 0x09, 0xB0, 0x57, 0xC5, 0x57, 18);
        public static WindowsDevicePropertyKeys PKEY_RecordedTV_IsRepeatBroadcast = new WindowsDevicePropertyKeys(0x6D748DE2, 0x8D38, 0x4CC3, 0xAC, 0x60, 0xF0, 0x09, 0xB0, 0x57, 0xC5, 0x57, 13);
        public static WindowsDevicePropertyKeys PKEY_RecordedTV_IsSAP = new WindowsDevicePropertyKeys(0x6D748DE2, 0x8D38, 0x4CC3, 0xAC, 0x60, 0xF0, 0x09, 0xB0, 0x57, 0xC5, 0x57, 14);
        public static WindowsDevicePropertyKeys PKEY_RecordedTV_NetworkAffiliation = new WindowsDevicePropertyKeys(0x2C53C813, 0xFB63, 0x4E22, 0xA1, 0xAB, 0x0B, 0x33, 0x1C, 0xA1, 0xE2, 0x73, 100);
        public static WindowsDevicePropertyKeys PKEY_RecordedTV_OriginalBroadcastDate = new WindowsDevicePropertyKeys(0x4684FE97, 0x8765, 0x4842, 0x9C, 0x13, 0xF0, 0x06, 0x44, 0x7B, 0x17, 0x8C, 100);
        public static WindowsDevicePropertyKeys PKEY_RecordedTV_ProgramDescription = new WindowsDevicePropertyKeys(0x6D748DE2, 0x8D38, 0x4CC3, 0xAC, 0x60, 0xF0, 0x09, 0xB0, 0x57, 0xC5, 0x57, 3);
        public static WindowsDevicePropertyKeys PKEY_RecordedTV_RecordingTime = new WindowsDevicePropertyKeys(0xA5477F61, 0x7A82, 0x4ECA, 0x9D, 0xDE, 0x98, 0xB6, 0x9B, 0x24, 0x79, 0xB3, 100);
        public static WindowsDevicePropertyKeys PKEY_RecordedTV_StationCallSign = new WindowsDevicePropertyKeys(0x6D748DE2, 0x8D38, 0x4CC3, 0xAC, 0x60, 0xF0, 0x09, 0xB0, 0x57, 0xC5, 0x57, 5);
        public static WindowsDevicePropertyKeys PKEY_RecordedTV_StationName = new WindowsDevicePropertyKeys(0x1B5439E7, 0xEBA1, 0x4AF8, 0xBD, 0xD7, 0x7A, 0xF1, 0xD4, 0x54, 0x94, 0x93, 100);
        public static WindowsDevicePropertyKeys PKEY_Search_AutoSummary = new WindowsDevicePropertyKeys(0x560C36C0, 0x503A, 0x11CF, 0xBA, 0xA1, 0x00, 0x00, 0x4C, 0x75, 0x2A, 0x9A, 2);
        public static WindowsDevicePropertyKeys PKEY_Search_ContainerHash = new WindowsDevicePropertyKeys(0xBCEEE283, 0x35DF, 0x4D53, 0x82, 0x6A, 0xF3, 0x6A, 0x3E, 0xEF, 0xC6, 0xBE, 100);
        public static WindowsDevicePropertyKeys PKEY_Search_Contents = new WindowsDevicePropertyKeys(0xB725F130, 0x47EF, 0x101A, 0xA5, 0xF1, 0x02, 0x60, 0x8C, 0x9E, 0xEB, 0xAC, 19);
        public static WindowsDevicePropertyKeys PKEY_Search_EntryID = new WindowsDevicePropertyKeys(0x49691C90, 0x7E17, 0x101A, 0xA9, 0x1C, 0x08, 0x00, 0x2B, 0x2E, 0xCD, 0xA9, 5);
        public static WindowsDevicePropertyKeys PKEY_Search_GatherTime = new WindowsDevicePropertyKeys(0x0B63E350, 0x9CCC, 0x11D0, 0xBC, 0xDB, 0x00, 0x80, 0x5F, 0xCC, 0xCE, 0x04, 8);
        public static WindowsDevicePropertyKeys PKEY_Search_IsClosedDirectory = new WindowsDevicePropertyKeys(0x0B63E343, 0x9CCC, 0x11D0, 0xBC, 0xDB, 0x00, 0x80, 0x5F, 0xCC, 0xCE, 0x04, 23);
        public static WindowsDevicePropertyKeys PKEY_Search_IsFullyContained = new WindowsDevicePropertyKeys(0x0B63E343, 0x9CCC, 0x11D0, 0xBC, 0xDB, 0x00, 0x80, 0x5F, 0xCC, 0xCE, 0x04, 24);
        public static WindowsDevicePropertyKeys PKEY_Search_QueryFocusedSummary = new WindowsDevicePropertyKeys(0x560C36C0, 0x503A, 0x11CF, 0xBA, 0xA1, 0x00, 0x00, 0x4C, 0x75, 0x2A, 0x9A, 3);
        public static WindowsDevicePropertyKeys PKEY_Search_Rank = new WindowsDevicePropertyKeys(0x49691C90, 0x7E17, 0x101A, 0xA9, 0x1C, 0x08, 0x00, 0x2B, 0x2E, 0xCD, 0xA9, 3);
        public static WindowsDevicePropertyKeys PKEY_Search_Store = new WindowsDevicePropertyKeys(0xA06992B3, 0x8CAF, 0x4ED7, 0xA5, 0x47, 0xB2, 0x59, 0xE3, 0x2A, 0xC9, 0xFC, 100);
        public static WindowsDevicePropertyKeys PKEY_Search_UrlToIndex = new WindowsDevicePropertyKeys(0x0B63E343, 0x9CCC, 0x11D0, 0xBC, 0xDB, 0x00, 0x80, 0x5F, 0xCC, 0xCE, 0x04, 2);
        public static WindowsDevicePropertyKeys PKEY_Search_UrlToIndexWithModificationTime = new WindowsDevicePropertyKeys(0x0B63E343, 0x9CCC, 0x11D0, 0xBC, 0xDB, 0x00, 0x80, 0x5F, 0xCC, 0xCE, 0x04, 12);
        public static WindowsDevicePropertyKeys PKEY_DescriptionID = new WindowsDevicePropertyKeys(0x28636AA6, 0x953D, 0x11D2, 0xB5, 0xD6, 0x00, 0xC0, 0x4F, 0xD9, 0x18, 0xD0, 2);
        public static WindowsDevicePropertyKeys PKEY_Link_TargetSFGAOFlagsStrings = new WindowsDevicePropertyKeys(0xD6942081, 0xD53B, 0x443D, 0xAD, 0x47, 0x5E, 0x05, 0x9D, 0x9C, 0xD2, 0x7A, 3);
        public static WindowsDevicePropertyKeys PKEY_Link_TargetUrl = new WindowsDevicePropertyKeys(0x5CBF2787, 0x48CF, 0x4208, 0xB9, 0x0E, 0xEE, 0x5E, 0x5D, 0x42, 0x02, 0x94, 2);
        public static WindowsDevicePropertyKeys PKEY_Shell_SFGAOFlagsStrings = new WindowsDevicePropertyKeys(0xD6942081, 0xD53B, 0x443D, 0xAD, 0x47, 0x5E, 0x05, 0x9D, 0x9C, 0xD2, 0x7A, 2);
        public static WindowsDevicePropertyKeys PKEY_Software_DateLastUsed = new WindowsDevicePropertyKeys(0x841E4F90, 0xFF59, 0x4D16, 0x89, 0x47, 0xE8, 0x1B, 0xBF, 0xFA, 0xB3, 0x6D, 16);
        public static WindowsDevicePropertyKeys PKEY_Software_ProductName = new WindowsDevicePropertyKeys(0x0CEF7D53, 0xFA64, 0x11D1, 0xA2, 0x03, 0x00, 0x00, 0xF8, 0x1F, 0xED, 0xEE, 7);
        public static WindowsDevicePropertyKeys PKEY_Sync_Comments = new WindowsDevicePropertyKeys(0x7BD5533E, 0xAF15, 0x44DB, 0xB8, 0xC8, 0xBD, 0x66, 0x24, 0xE1, 0xD0, 0x32, 13);
        public static WindowsDevicePropertyKeys PKEY_Sync_ConflictDescription = new WindowsDevicePropertyKeys(0xCE50C159, 0x2FB8, 0x41FD, 0xBE, 0x68, 0xD3, 0xE0, 0x42, 0xE2, 0x74, 0xBC, 4);
        public static WindowsDevicePropertyKeys PKEY_Sync_ConflictFirstLocation = new WindowsDevicePropertyKeys(0xCE50C159, 0x2FB8, 0x41FD, 0xBE, 0x68, 0xD3, 0xE0, 0x42, 0xE2, 0x74, 0xBC, 6);
        public static WindowsDevicePropertyKeys PKEY_Sync_ConflictSecondLocation = new WindowsDevicePropertyKeys(0xCE50C159, 0x2FB8, 0x41FD, 0xBE, 0x68, 0xD3, 0xE0, 0x42, 0xE2, 0x74, 0xBC, 7);
        public static WindowsDevicePropertyKeys PKEY_Sync_HandlerCollectionID = new WindowsDevicePropertyKeys(0x7BD5533E, 0xAF15, 0x44DB, 0xB8, 0xC8, 0xBD, 0x66, 0x24, 0xE1, 0xD0, 0x32, 2);
        public static WindowsDevicePropertyKeys PKEY_Sync_HandlerID = new WindowsDevicePropertyKeys(0x7BD5533E, 0xAF15, 0x44DB, 0xB8, 0xC8, 0xBD, 0x66, 0x24, 0xE1, 0xD0, 0x32, 3);
        public static WindowsDevicePropertyKeys PKEY_Sync_HandlerName = new WindowsDevicePropertyKeys(0xCE50C159, 0x2FB8, 0x41FD, 0xBE, 0x68, 0xD3, 0xE0, 0x42, 0xE2, 0x74, 0xBC, 2);
        public static WindowsDevicePropertyKeys PKEY_Sync_HandlerType = new WindowsDevicePropertyKeys(0x7BD5533E, 0xAF15, 0x44DB, 0xB8, 0xC8, 0xBD, 0x66, 0x24, 0xE1, 0xD0, 0x32, 8);
        public static WindowsDevicePropertyKeys PKEY_Sync_HandlerTypeLabel = new WindowsDevicePropertyKeys(0x7BD5533E, 0xAF15, 0x44DB, 0xB8, 0xC8, 0xBD, 0x66, 0x24, 0xE1, 0xD0, 0x32, 9);
        public static WindowsDevicePropertyKeys PKEY_Sync_ItemID = new WindowsDevicePropertyKeys(0x7BD5533E, 0xAF15, 0x44DB, 0xB8, 0xC8, 0xBD, 0x66, 0x24, 0xE1, 0xD0, 0x32, 6);
        public static WindowsDevicePropertyKeys PKEY_Sync_ItemName = new WindowsDevicePropertyKeys(0xCE50C159, 0x2FB8, 0x41FD, 0xBE, 0x68, 0xD3, 0xE0, 0x42, 0xE2, 0x74, 0xBC, 3);
        public static WindowsDevicePropertyKeys PKEY_Task_BillingInformation = new WindowsDevicePropertyKeys(0xD37D52C6, 0x261C, 0x4303, 0x82, 0xB3, 0x08, 0xB9, 0x26, 0xAC, 0x6F, 0x12, 100);
        public static WindowsDevicePropertyKeys PKEY_Task_CompletionStatus = new WindowsDevicePropertyKeys(0x084D8A0A, 0xE6D5, 0x40DE, 0xBF, 0x1F, 0xC8, 0x82, 0x0E, 0x7C, 0x87, 0x7C, 100);
        public static WindowsDevicePropertyKeys PKEY_Task_Owner = new WindowsDevicePropertyKeys(0x08C7CC5F, 0x60F2, 0x4494, 0xAD, 0x75, 0x55, 0xE3, 0xE0, 0xB5, 0xAD, 0xD0, 100);
        public static WindowsDevicePropertyKeys PKEY_Video_Compression = new WindowsDevicePropertyKeys(0x64440491, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 10);
        public static WindowsDevicePropertyKeys PKEY_Video_Director = new WindowsDevicePropertyKeys(0x64440492, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 20);
        public static WindowsDevicePropertyKeys PKEY_Video_EncodingBitrate = new WindowsDevicePropertyKeys(0x64440491, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 8);
        public static WindowsDevicePropertyKeys PKEY_Video_FourCC = new WindowsDevicePropertyKeys(0x64440491, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 44);
        public static WindowsDevicePropertyKeys PKEY_Video_FrameHeight = new WindowsDevicePropertyKeys(0x64440491, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 4);
        public static WindowsDevicePropertyKeys PKEY_Video_FrameRate = new WindowsDevicePropertyKeys(0x64440491, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 6);
        public static WindowsDevicePropertyKeys PKEY_Video_FrameWidth = new WindowsDevicePropertyKeys(0x64440491, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 3);
        public static WindowsDevicePropertyKeys PKEY_Video_HorizontalAspectRatio = new WindowsDevicePropertyKeys(0x64440491, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 42);
        public static WindowsDevicePropertyKeys PKEY_Video_SampleSize = new WindowsDevicePropertyKeys(0x64440491, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 9);
        public static WindowsDevicePropertyKeys PKEY_Video_StreamName = new WindowsDevicePropertyKeys(0x64440491, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 2);
        public static WindowsDevicePropertyKeys PKEY_Video_StreamNumber = new WindowsDevicePropertyKeys(0x64440491, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 11);
        public static WindowsDevicePropertyKeys PKEY_Video_TotalBitrate = new WindowsDevicePropertyKeys(0x64440491, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 43);
        public static WindowsDevicePropertyKeys PKEY_Video_VerticalAspectRatio = new WindowsDevicePropertyKeys(0x64440491, 0x4C8B, 0x11D1, 0x8B, 0x70, 0x08, 0x00, 0x36, 0xB1, 0x1A, 0x03, 45);
        public static WindowsDevicePropertyKeys PKEY_Volume_FileSystem = new WindowsDevicePropertyKeys(0x9B174B35, 0x40FF, 0x11D2, 0xA2, 0x7E, 0x00, 0xC0, 0x4F, 0xC3, 0x08, 0x71, 4);
        public static WindowsDevicePropertyKeys PKEY_Volume_IsMappedDrive = new WindowsDevicePropertyKeys(0x149C0B69, 0x2C2D, 0x48FC, 0x80, 0x8F, 0xD3, 0x18, 0xD7, 0x8C, 0x46, 0x36, 2);
        public static WindowsDevicePropertyKeys PKEY_Volume_IsRoot = new WindowsDevicePropertyKeys(0x9B174B35, 0x40FF, 0x11D2, 0xA2, 0x7E, 0x00, 0xC0, 0x4F, 0xC3, 0x08, 0x71, 10);

        public static WindowsDevicePropertyKeys PKEY_AudioEndpoint_FormFactor = new WindowsDevicePropertyKeys(0x1da5d803, 0xd492, 0x4edd, 0x8c, 0x23, 0xe0, 0xc0, 0xff, 0xee, 0x7f, 0x0e, 0);
        public static WindowsDevicePropertyKeys PKEY_AudioEndpoint_ControlPanelPageProvider = new WindowsDevicePropertyKeys(0x1da5d803, 0xd492, 0x4edd, 0x8c, 0x23, 0xe0, 0xc0, 0xff, 0xee, 0x7f, 0x0e, 1);
        public static WindowsDevicePropertyKeys PKEY_AudioEndpoint_Association = new WindowsDevicePropertyKeys(0x1da5d803, 0xd492, 0x4edd, 0x8c, 0x23, 0xe0, 0xc0, 0xff, 0xee, 0x7f, 0x0e, 2);
        public static WindowsDevicePropertyKeys PKEY_AudioEndpoint_PhysicalSpeakers = new WindowsDevicePropertyKeys(0x1da5d803, 0xd492, 0x4edd, 0x8c, 0x23, 0xe0, 0xc0, 0xff, 0xee, 0x7f, 0x0e, 3);
        public static WindowsDevicePropertyKeys PKEY_AudioEndpoint_GUID = new WindowsDevicePropertyKeys(0x1da5d803, 0xd492, 0x4edd, 0x8c, 0x23, 0xe0, 0xc0, 0xff, 0xee, 0x7f, 0x0e, 4);
        public static WindowsDevicePropertyKeys PKEY_AudioEndpoint_Disable_SysFx = new WindowsDevicePropertyKeys(0x1da5d803, 0xd492, 0x4edd, 0x8c, 0x23, 0xe0, 0xc0, 0xff, 0xee, 0x7f, 0x0e, 5);
        public static WindowsDevicePropertyKeys PKEY_AudioEndpoint_FullRangeSpeakers = new WindowsDevicePropertyKeys(0x1da5d803, 0xd492, 0x4edd, 0x8c, 0x23, 0xe0, 0xc0, 0xff, 0xee, 0x7f, 0x0e, 6);
        public static WindowsDevicePropertyKeys PKEY_AudioEndpoint_Supports_EventDriven_Mode = new WindowsDevicePropertyKeys(0x1da5d803, 0xd492, 0x4edd, 0x8c, 0x23, 0xe0, 0xc0, 0xff, 0xee, 0x7f, 0x0e, 7);
        public static WindowsDevicePropertyKeys PKEY_AudioEndpoint_JackSubType = new WindowsDevicePropertyKeys(0x1da5d803, 0xd492, 0x4edd, 0x8c, 0x23, 0xe0, 0xc0, 0xff, 0xee, 0x7f, 0x0e, 8);
        public static WindowsDevicePropertyKeys PKEY_AudioEngine_DeviceFormat = new WindowsDevicePropertyKeys(0xf19f064d, 0x82c, 0x4e27, 0xbc, 0x73, 0x68, 0x82, 0xa1, 0xbb, 0x8e, 0x4c, 0);
        public static WindowsDevicePropertyKeys PKEY_AudioEngine_OEMFormat = new WindowsDevicePropertyKeys(0xe4870e26, 0x3cc5, 0x4cd2, 0xba, 0x46, 0xca, 0xa, 0x9a, 0x70, 0xed, 0x4, 3);

        public static WindowsDevicePropertyKeys PKEY_FunctionInstance = new WindowsDevicePropertyKeys(0x08c0c253, 0xa154, 0x4746, 0x90, 0x05, 0x82, 0xde, 0x53, 0x17, 0x14, 0x8b, 0x00000001);  // VT_UNKNOWN
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_Address = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000033);  // VT_LPWSTR or VT_LPWSTR | VT_VECTOR
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_DiscoveryMethod = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000034);  // VT_LPWSTR | VT_VECTOR
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_IsEncrypted = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000035);  // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_IsAuthenticated = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000036);  // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_IsConnected = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000037);  // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_IsPaired = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000038);  // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_Icon = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000039);  // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_Version = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000041);  // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_Last_Seen = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000042);  // VT_FIELTIME
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_Last_Connected = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000043);  // VT_FILETIME
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_IsShowInDisconnectedState = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000044);  // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_IsLocalMachine = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000046);  // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_MetadataPath = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000047);  // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_IsMetadataSearchInProgress = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000048);  // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_MetadataChecksum = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000049);  // VT_UI1 | VT_VECTOR
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_IsNotInterestingForDisplay = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x0000004A);  // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_LaunchDeviceStageOnDeviceConnect = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x0000004C);  // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_LaunchDeviceStageFromExplorer = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x0000004D);  // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_BaselineExperienceId = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x0000004E);  // VT_CLSID
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_IsDeviceUniquelyIdentifiable = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x0000004F);  // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_AssociationArray = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000050);  // VT_LPWSTR  | VT_VECTOR
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_DeviceDescription1 = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000051);  // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_DeviceDescription2 = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000052);  // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_IsNotWorkingProperly = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000053);  // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_IsSharedDevice = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000054);  // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_IsNetworkDevice = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000055);  // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_IsDefaultDevice = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000056);  // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_MetadataCabinet = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000057);  // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_RequiresPairingElevation = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000058);  // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_ExperienceId = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000059);  // VT_CLSID
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_Category = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x0000005A);  // VT_LPWSTR | VT_VECTOR
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_Category_Desc_Singular = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x0000005B);  // VT_LPWSTR | VT_VECTOR
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_Category_Desc_Plural = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x0000005C);  // VT_LPWSTR | VT_VECTOR
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_Category_Icon = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x0000005D);  // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_CategoryGroup_Desc = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x0000005E);  // VT_LPWSTR | VT_VECTOR
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_CategoryGroup_Icon = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x0000005F);  // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_PrimaryCategory = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000061);  // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_UnpairUninstall = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000062);  // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_RequiresUninstallElevation = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000063);  // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_DeviceFunctionSubRank = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000064);  // VT_UI4
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_AlwaysShowDeviceAsConnected = new WindowsDevicePropertyKeys(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x00000065);  // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_FriendlyName = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00003000);  // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_Manufacturer = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00002000);  // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_ModelName = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00002002);  // VT_LPWSTR (localizable)
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_ModelNumber = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00002003);  // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_DeviceDisplay_InstallInProgress = new WindowsDevicePropertyKeys(0x83da6326, 0x97a6, 0x4088, 0x94, 0x53, 0xa1, 0x92, 0x3f, 0x57, 0x3b, 0x29, 9);     // DEVPROP_TYPE_BOOLEAN
        public static WindowsDevicePropertyKeys PKEY_Pairing_ListItemText = new WindowsDevicePropertyKeys(0x8807cae6, 0x7db6, 0x4f10, 0x8e, 0xe4, 0x43, 0x5e, 0xaa, 0x13, 0x92, 0xbc, 0x0000001);    // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_Pairing_ListItemDescription = new WindowsDevicePropertyKeys(0x8807cae6, 0x7db6, 0x4f10, 0x8e, 0xe4, 0x43, 0x5e, 0xaa, 0x13, 0x92, 0xbc, 0x0000002); // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_Pairing_ListItemIcon = new WindowsDevicePropertyKeys(0x8807cae6, 0x7db6, 0x4f10, 0x8e, 0xe4, 0x43, 0x5e, 0xaa, 0x13, 0x92, 0xbc, 0x0000003);    // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_Pairing_ListItemDefault = new WindowsDevicePropertyKeys(0x8807cae6, 0x7db6, 0x4f10, 0x8e, 0xe4, 0x43, 0x5e, 0xaa, 0x13, 0x92, 0xbc, 0x0000004);     // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_Pairing_IsWifiOnlyDevice = new WindowsDevicePropertyKeys(0x8807cae6, 0x7db6, 0x4f10, 0x8e, 0xe4, 0x43, 0x5e, 0xaa, 0x13, 0x92, 0xbc, 0x0000010);    // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_Device_BIOSVersion = new WindowsDevicePropertyKeys(0xEAEE7F1D, 0x6A33, 0x44D1, 0x94, 0x41, 0x5F, 0x46, 0xDE, 0xF2, 0x31, 0x98, 9);
        public static WindowsDevicePropertyKeys PKEY_PNPX_GlobalIdentity = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00001000);   // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_PNPX_Types = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00001001);   // VT_LPWSTR | VT_VECTOR
        public static WindowsDevicePropertyKeys PKEY_PNPX_Scopes = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00001002);   // VT_LPWSTR | VT_VECTOR
        public static WindowsDevicePropertyKeys PKEY_PNPX_XAddrs = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00001003);   // VT_LPWSTR | VT_VECTOR
        public static WindowsDevicePropertyKeys PKEY_PNPX_MetadataVersion = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00001004);   // VT_UI8
        public static WindowsDevicePropertyKeys PKEY_PNPX_ID = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00001005);   // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_PNPX_RemoteAddress = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00001006);   // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_PNPX_RootProxy = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00001007);   // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_PNPX_ManufacturerUrl = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00002001);  // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_PNPX_ModelUrl = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00002004);  // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_PNPX_Upc = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00002005);   // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_PNPX_PresentationUrl = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00002006);   // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_PNPX_FirmwareVersion = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00003001);   // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_PNPX_SerialNumber = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00003002);   // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_PNPX_DeviceCategory = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00003004);  // VT_LPWSTR | VT_VECTOR
        public static WindowsDevicePropertyKeys PKEY_PNPX_SecureChannel = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00007001);   // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_PNPX_CompactSignature = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00007002);   // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_PNPX_DeviceCategory_Desc = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00003005);   // VT_LPWSTR | VT_VECTOR
        public static WindowsDevicePropertyKeys PKEY_PNPX_Category_Desc_NonPlural = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00003010);   // VT_LPWSTR | VT_VECTOR
        public static WindowsDevicePropertyKeys PKEY_PNPX_PhysicalAddress = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00003006);   // VT_UI1 | VT_VECTOR
        public static WindowsDevicePropertyKeys PKEY_PNPX_NetworkInterfaceLuid = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00003007);   // VT_UI8
        public static WindowsDevicePropertyKeys PKEY_PNPX_NetworkInterfaceGuid = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00003008);   // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_PNPX_IpAddress = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00003009);   // VT_LPWSTR | VT_VECTOR
        public static WindowsDevicePropertyKeys PKEY_PNPX_ServiceAddress = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00004000);   // VT_LPWSTR | VT_VECTOR
        public static WindowsDevicePropertyKeys PKEY_PNPX_ServiceId = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00004001);   // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_PNPX_ServiceTypes = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00004002);   // VT_LPWSTR | VT_VECTOR
        public static WindowsDevicePropertyKeys PKEY_PNPX_DomainName = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00005000);   // VT_LPWSTR
                                                                                                                                                                    // Use PKEY_ComputerName (propkey.h)     public static PropertyKeyLookup PKEY_PNPX_MachineName = new PropertyKeyLookup( 0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00005001);   // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_PNPX_ShareName = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00005002);   // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_SSDP_AltLocationInfo = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00006000);   // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_SSDP_DevLifeTime = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00006001);   // VT_UI4
        public static WindowsDevicePropertyKeys PKEY_SSDP_NetworkInterface = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00006002);   // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_PNPX_Installable = new WindowsDevicePropertyKeys(0x4FC5077E, 0xB686, 0x44BE, 0x93, 0xE3, 0x86, 0xCA, 0xFE, 0x36, 0x8C, 0xCD, 0x00000001); // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_PNPX_Associated = new WindowsDevicePropertyKeys(0x4FC5077E, 0xB686, 0x44BE, 0x93, 0xE3, 0x86, 0xCA, 0xFE, 0x36, 0x8C, 0xCD, 0x00000002); // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_PNPX_CompatibleTypes = new WindowsDevicePropertyKeys(0x4FC5077E, 0xB686, 0x44BE, 0x93, 0xE3, 0x86, 0xCA, 0xFE, 0x36, 0x8C, 0xCD, 0x00000003); // VT_LPWSTR | VT_VECTOR
        public static WindowsDevicePropertyKeys PKEY_PNPX_InstallState = new WindowsDevicePropertyKeys(0x4FC5077E, 0xB686, 0x44BE, 0x93, 0xE3, 0x86, 0xCA, 0xFE, 0x36, 0x8C, 0xCD, 0x00000004); // VT_UI4 | VT_VECTOR
        public static WindowsDevicePropertyKeys PKEY_PNPX_Removable = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00007000);   // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_PNPX_IPBusEnumerated = new WindowsDevicePropertyKeys(0x656A3BB3, 0xECC0, 0x43FD, 0x84, 0x77, 0x4A, 0xE0, 0x40, 0x4A, 0x96, 0xCD, 0x00007010);   // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_WNET_Scope = new WindowsDevicePropertyKeys(0xdebda43a, 0x37b3, 0x4383, 0x91, 0xE7, 0x44, 0x98, 0xda, 0x29, 0x95, 0xab, 0x00000001); // VT_UINT
        public static WindowsDevicePropertyKeys PKEY_WNET_Type = new WindowsDevicePropertyKeys(0xdebda43a, 0x37b3, 0x4383, 0x91, 0xE7, 0x44, 0x98, 0xda, 0x29, 0x95, 0xab, 0x00000002); // VT_UINT
        public static WindowsDevicePropertyKeys PKEY_WNET_DisplayType = new WindowsDevicePropertyKeys(0xdebda43a, 0x37b3, 0x4383, 0x91, 0xE7, 0x44, 0x98, 0xda, 0x29, 0x95, 0xab, 0x00000003); // VT_UINT
        public static WindowsDevicePropertyKeys PKEY_WNET_Usage = new WindowsDevicePropertyKeys(0xdebda43a, 0x37b3, 0x4383, 0x91, 0xE7, 0x44, 0x98, 0xda, 0x29, 0x95, 0xab, 0x00000004); // VT_UINT
        public static WindowsDevicePropertyKeys PKEY_WNET_LocalName = new WindowsDevicePropertyKeys(0xdebda43a, 0x37b3, 0x4383, 0x91, 0xE7, 0x44, 0x98, 0xda, 0x29, 0x95, 0xab, 0x00000005); // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_WNET_RemoteName = new WindowsDevicePropertyKeys(0xdebda43a, 0x37b3, 0x4383, 0x91, 0xE7, 0x44, 0x98, 0xda, 0x29, 0x95, 0xab, 0x00000006); // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_WNET_Comment = new WindowsDevicePropertyKeys(0xdebda43a, 0x37b3, 0x4383, 0x91, 0xE7, 0x44, 0x98, 0xda, 0x29, 0x95, 0xab, 0x00000007); // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_WNET_Provider = new WindowsDevicePropertyKeys(0xdebda43a, 0x37b3, 0x4383, 0x91, 0xE7, 0x44, 0x98, 0xda, 0x29, 0x95, 0xab, 0x00000008); // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_WCN_Version = new WindowsDevicePropertyKeys(0x88190b80, 0x4684, 0x11da, 0xa2, 0x6a, 0x00, 0x02, 0xb3, 0x98, 0x8e, 0x81, 0x00000001); // VT_UI1
        public static WindowsDevicePropertyKeys PKEY_WCN_RequestType = new WindowsDevicePropertyKeys(0x88190b81, 0x4684, 0x11da, 0xa2, 0x6a, 0x00, 0x02, 0xb3, 0x98, 0x8e, 0x81, 0x00000002); // VT_INT
        public static WindowsDevicePropertyKeys PKEY_WCN_AuthType = new WindowsDevicePropertyKeys(0x88190b82, 0x4684, 0x11da, 0xa2, 0x6a, 0x00, 0x02, 0xb3, 0x98, 0x8e, 0x81, 0x00000003); // VT_INT
        public static WindowsDevicePropertyKeys PKEY_WCN_EncryptType = new WindowsDevicePropertyKeys(0x88190b83, 0x4684, 0x11da, 0xa2, 0x6a, 0x00, 0x02, 0xb3, 0x98, 0x8e, 0x81, 0x00000004); // VT_INT
        public static WindowsDevicePropertyKeys PKEY_WCN_ConnType = new WindowsDevicePropertyKeys(0x88190b84, 0x4684, 0x11da, 0xa2, 0x6a, 0x00, 0x02, 0xb3, 0x98, 0x8e, 0x81, 0x00000005); // VT_INT
        public static WindowsDevicePropertyKeys PKEY_WCN_ConfigMethods = new WindowsDevicePropertyKeys(0x88190b85, 0x4684, 0x11da, 0xa2, 0x6a, 0x00, 0x02, 0xb3, 0x98, 0x8e, 0x81, 0x00000006); // VT_INT
                                                                                                                                                                    //public static PropertyKeyLookup PKEY_WCN_DeviceType = new PropertyKeyLookup( 0x88190b86, 0x4684, 0x11da, 0xa2, 0x6a, 0x00, 0x02, 0xb3, 0x98, 0x8e, 0x81, 0x00000007); // VT_INT
        public static WindowsDevicePropertyKeys PKEY_WCN_RfBand = new WindowsDevicePropertyKeys(0x88190b87, 0x4684, 0x11da, 0xa2, 0x6a, 0x00, 0x02, 0xb3, 0x98, 0x8e, 0x81, 0x00000008); // VT_INT
        public static WindowsDevicePropertyKeys PKEY_WCN_AssocState = new WindowsDevicePropertyKeys(0x88190b88, 0x4684, 0x11da, 0xa2, 0x6a, 0x00, 0x02, 0xb3, 0x98, 0x8e, 0x81, 0x00000009); // VT_INT
        public static WindowsDevicePropertyKeys PKEY_WCN_ConfigError = new WindowsDevicePropertyKeys(0x88190b89, 0x4684, 0x11da, 0xa2, 0x6a, 0x00, 0x02, 0xb3, 0x98, 0x8e, 0x81, 0x0000000a); // VT_INT
        public static WindowsDevicePropertyKeys PKEY_WCN_ConfigState = new WindowsDevicePropertyKeys(0x88190b89, 0x4684, 0x11da, 0xa2, 0x6a, 0x00, 0x02, 0xb3, 0x98, 0x8e, 0x81, 0x0000000b); // VT_UI1
        public static WindowsDevicePropertyKeys PKEY_WCN_DevicePasswordId = new WindowsDevicePropertyKeys(0x88190b89, 0x4684, 0x11da, 0xa2, 0x6a, 0x00, 0x02, 0xb3, 0x98, 0x8e, 0x81, 0x0000000c); // VT_INT
        public static WindowsDevicePropertyKeys PKEY_WCN_OSVersion = new WindowsDevicePropertyKeys(0x88190b89, 0x4684, 0x11da, 0xa2, 0x6a, 0x00, 0x02, 0xb3, 0x98, 0x8e, 0x81, 0x0000000d); // VT_UINT
        public static WindowsDevicePropertyKeys PKEY_WCN_VendorExtension = new WindowsDevicePropertyKeys(0x88190b8a, 0x4684, 0x11da, 0xa2, 0x6a, 0x00, 0x02, 0xb3, 0x98, 0x8e, 0x81, 0x0000000e); // VT_UI1 | VT_VECTOR
        public static WindowsDevicePropertyKeys PKEY_WCN_RegistrarType = new WindowsDevicePropertyKeys(0x88190b8b, 0x4684, 0x11da, 0xa2, 0x6a, 0x00, 0x02, 0xb3, 0x98, 0x8e, 0x81, 0x0000000f); // VT_INT
        public static WindowsDevicePropertyKeys PKEY_Hardware_Devinst = new WindowsDevicePropertyKeys(0x5EAF3EF2, 0xE0CA, 0x4598, 0xBF, 0x06, 0x71, 0xED, 0x1D, 0x9D, 0xD9, 0x53, 4097);
        public static WindowsDevicePropertyKeys PKEY_Hardware_DisplayAttribute = new WindowsDevicePropertyKeys(0x5EAF3EF2, 0xE0CA, 0x4598, 0xBF, 0x06, 0x71, 0xED, 0x1D, 0x9D, 0xD9, 0x53, 5);
        public static WindowsDevicePropertyKeys PKEY_Hardware_DriverDate = new WindowsDevicePropertyKeys(0x5EAF3EF2, 0xE0CA, 0x4598, 0xBF, 0x06, 0x71, 0xED, 0x1D, 0x9D, 0xD9, 0x53, 11);
        public static WindowsDevicePropertyKeys PKEY_Hardware_DriverProvider = new WindowsDevicePropertyKeys(0x5EAF3EF2, 0xE0CA, 0x4598, 0xBF, 0x06, 0x71, 0xED, 0x1D, 0x9D, 0xD9, 0x53, 10);
        public static WindowsDevicePropertyKeys PKEY_Hardware_DriverVersion = new WindowsDevicePropertyKeys(0x5EAF3EF2, 0xE0CA, 0x4598, 0xBF, 0x06, 0x71, 0xED, 0x1D, 0x9D, 0xD9, 0x53, 9);
        public static WindowsDevicePropertyKeys PKEY_Hardware_Function = new WindowsDevicePropertyKeys(0x5EAF3EF2, 0xE0CA, 0x4598, 0xBF, 0x06, 0x71, 0xED, 0x1D, 0x9D, 0xD9, 0x53, 4099);
        public static WindowsDevicePropertyKeys PKEY_Hardware_Icon = new WindowsDevicePropertyKeys(0x5EAF3EF2, 0xE0CA, 0x4598, 0xBF, 0x06, 0x71, 0xED, 0x1D, 0x9D, 0xD9, 0x53, 3);
        public static WindowsDevicePropertyKeys PKEY_Hardware_Image = new WindowsDevicePropertyKeys(0x5EAF3EF2, 0xE0CA, 0x4598, 0xBF, 0x06, 0x71, 0xED, 0x1D, 0x9D, 0xD9, 0x53, 4098);
        public static WindowsDevicePropertyKeys PKEY_Hardware_Manufacturer = new WindowsDevicePropertyKeys(0x5EAF3EF2, 0xE0CA, 0x4598, 0xBF, 0x06, 0x71, 0xED, 0x1D, 0x9D, 0xD9, 0x53, 6);
        public static WindowsDevicePropertyKeys PKEY_Hardware_Model = new WindowsDevicePropertyKeys(0x5EAF3EF2, 0xE0CA, 0x4598, 0xBF, 0x06, 0x71, 0xED, 0x1D, 0x9D, 0xD9, 0x53, 7);
        public static WindowsDevicePropertyKeys PKEY_Hardware_Name = new WindowsDevicePropertyKeys(0x5EAF3EF2, 0xE0CA, 0x4598, 0xBF, 0x06, 0x71, 0xED, 0x1D, 0x9D, 0xD9, 0x53, 2);
        public static WindowsDevicePropertyKeys PKEY_Hardware_SerialNumber = new WindowsDevicePropertyKeys(0x5EAF3EF2, 0xE0CA, 0x4598, 0xBF, 0x06, 0x71, 0xED, 0x1D, 0x9D, 0xD9, 0x53, 8);
        public static WindowsDevicePropertyKeys PKEY_Hardware_ShellAttributes = new WindowsDevicePropertyKeys(0x5EAF3EF2, 0xE0CA, 0x4598, 0xBF, 0x06, 0x71, 0xED, 0x1D, 0x9D, 0xD9, 0x53, 4100);
        public static WindowsDevicePropertyKeys PKEY_Hardware_Status = new WindowsDevicePropertyKeys(0x5EAF3EF2, 0xE0CA, 0x4598, 0xBF, 0x06, 0x71, 0xED, 0x1D, 0x9D, 0xD9, 0x53, 4096);
        public static WindowsDevicePropertyKeys PKEY_PhotoAcquire_RelativePathname = new WindowsDevicePropertyKeys(0x00f23377, 0x7ac6, 0x4b7a, 0x84, 0x43, 0x34, 0x5e, 0x73, 0x1f, 0xa5, 0x7a, 2); // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_PhotoAcquire_FinalFilename = new WindowsDevicePropertyKeys(0x00f23377, 0x7ac6, 0x4b7a, 0x84, 0x43, 0x34, 0x5e, 0x73, 0x1f, 0xa5, 0x7a, 3);    // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_PhotoAcquire_GroupTag = new WindowsDevicePropertyKeys(0x00f23377, 0x7ac6, 0x4b7a, 0x84, 0x43, 0x34, 0x5e, 0x73, 0x1f, 0xa5, 0x7a, 4);    // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_PhotoAcquire_TransferResult = new WindowsDevicePropertyKeys(0x00f23377, 0x7ac6, 0x4b7a, 0x84, 0x43, 0x34, 0x5e, 0x73, 0x1f, 0xa5, 0x7a, 5);    // VT_SCODE
        public static WindowsDevicePropertyKeys PKEY_PhotoAcquire_OriginalFilename = new WindowsDevicePropertyKeys(0x00f23377, 0x7ac6, 0x4b7a, 0x84, 0x43, 0x34, 0x5e, 0x73, 0x1f, 0xa5, 0x7a, 6);    // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_PhotoAcquire_CameraSequenceNumber = new WindowsDevicePropertyKeys(0x00f23377, 0x7ac6, 0x4b7a, 0x84, 0x43, 0x34, 0x5e, 0x73, 0x1f, 0xa5, 0x7a, 7);    // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_PhotoAcquire_IntermediateFile = new WindowsDevicePropertyKeys(0x00f23377, 0x7ac6, 0x4b7a, 0x84, 0x43, 0x34, 0x5e, 0x73, 0x1f, 0xa5, 0x7a, 8);    // VT_LPWSTR
        public static WindowsDevicePropertyKeys PKEY_PhotoAcquire_SkipImport = new WindowsDevicePropertyKeys(0x00f23377, 0x7ac6, 0x4b7a, 0x84, 0x43, 0x34, 0x5e, 0x73, 0x1f, 0xa5, 0x7a, 9);    // VT_BOOL
        public static WindowsDevicePropertyKeys PKEY_PhotoAcquire_DuplicateDetectionID = new WindowsDevicePropertyKeys(0x00f23377, 0x7ac6, 0x4b7a, 0x84, 0x43, 0x34, 0x5e, 0x73, 0x1f, 0xa5, 0x7a, 10);    // VT_I4
        public static WindowsDevicePropertyKeys WPD_OBJECT_ID = new WindowsDevicePropertyKeys(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC, 0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C, 2);
        public static WindowsDevicePropertyKeys WPD_OBJECT_PARENT_ID = new WindowsDevicePropertyKeys(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC, 0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C, 3);
        public static WindowsDevicePropertyKeys WPD_OBJECT_NAME = new WindowsDevicePropertyKeys(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC, 0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C, 4);
        public static WindowsDevicePropertyKeys WPD_OBJECT_PERSISTENT_UNIQUE_ID = new WindowsDevicePropertyKeys(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC, 0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C, 5);
        public static WindowsDevicePropertyKeys WPD_OBJECT_FORMAT = new WindowsDevicePropertyKeys(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC, 0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C, 6);
        public static WindowsDevicePropertyKeys WPD_OBJECT_CONTENT_TYPE = new WindowsDevicePropertyKeys(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC, 0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C, 7);
        public static WindowsDevicePropertyKeys WPD_OBJECT_ISHIDDEN = new WindowsDevicePropertyKeys(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC, 0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C, 9);
        public static WindowsDevicePropertyKeys WPD_OBJECT_ISSYSTEM = new WindowsDevicePropertyKeys(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC, 0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C, 10);
        public static WindowsDevicePropertyKeys WPD_OBJECT_SIZE = new WindowsDevicePropertyKeys(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC, 0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C, 11);
        public static WindowsDevicePropertyKeys WPD_OBJECT_ORIGINAL_FILE_NAME = new WindowsDevicePropertyKeys(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC, 0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C, 12);
        public static WindowsDevicePropertyKeys WPD_OBJECT_NON_CONSUMABLE = new WindowsDevicePropertyKeys(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC, 0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C, 13);
        public static WindowsDevicePropertyKeys WPD_OBJECT_REFERENCES = new WindowsDevicePropertyKeys(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC, 0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C, 14);
        public static WindowsDevicePropertyKeys WPD_OBJECT_KEYWORDS = new WindowsDevicePropertyKeys(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC, 0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C, 15);
        public static WindowsDevicePropertyKeys WPD_OBJECT_SYNC_ID = new WindowsDevicePropertyKeys(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC, 0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C, 16);
        public static WindowsDevicePropertyKeys WPD_OBJECT_IS_DRM_PROTECTED = new WindowsDevicePropertyKeys(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC, 0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C, 17);
        public static WindowsDevicePropertyKeys WPD_OBJECT_DATE_CREATED = new WindowsDevicePropertyKeys(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC, 0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C, 18);
        public static WindowsDevicePropertyKeys WPD_OBJECT_DATE_MODIFIED = new WindowsDevicePropertyKeys(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC, 0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C, 19);
        public static WindowsDevicePropertyKeys WPD_OBJECT_DATE_AUTHORED = new WindowsDevicePropertyKeys(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC, 0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C, 20);
        public static WindowsDevicePropertyKeys WPD_OBJECT_BACK_REFERENCES = new WindowsDevicePropertyKeys(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC, 0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C, 21);
        public static WindowsDevicePropertyKeys WPD_OBJECT_CONTAINER_FUNCTIONAL_OBJECT_ID = new WindowsDevicePropertyKeys(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC, 0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C, 23);
        public static WindowsDevicePropertyKeys WPD_OBJECT_GENERATE_THUMBNAIL_FROM_RESOURCE = new WindowsDevicePropertyKeys(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC, 0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C, 24);
        public static WindowsDevicePropertyKeys WPD_OBJECT_HINT_LOCATION_DISPLAY_NAME = new WindowsDevicePropertyKeys(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC, 0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C, 25);
        public static WindowsDevicePropertyKeys WPD_OBJECT_CAN_DELETE = new WindowsDevicePropertyKeys(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC, 0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C, 26);
        public static WindowsDevicePropertyKeys WPD_OBJECT_LANGUAGE_LOCALE = new WindowsDevicePropertyKeys(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC, 0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C, 27);
        public static WindowsDevicePropertyKeys WPD_FOLDER_CONTENT_TYPES_ALLOWED = new WindowsDevicePropertyKeys(0x7E9A7ABF, 0xE568, 0x4B34, 0xAA, 0x2F, 0x13, 0xBB, 0x12, 0xAB, 0x17, 0x7D, 2);
        public static WindowsDevicePropertyKeys WPD_IMAGE_BITDEPTH = new WindowsDevicePropertyKeys(0x63D64908, 0x9FA1, 0x479F, 0x85, 0xBA, 0x99, 0x52, 0x21, 0x64, 0x47, 0xDB, 3);
        public static WindowsDevicePropertyKeys WPD_IMAGE_CROPPED_STATUS = new WindowsDevicePropertyKeys(0x63D64908, 0x9FA1, 0x479F, 0x85, 0xBA, 0x99, 0x52, 0x21, 0x64, 0x47, 0xDB, 4);
        public static WindowsDevicePropertyKeys WPD_IMAGE_COLOR_CORRECTED_STATUS = new WindowsDevicePropertyKeys(0x63D64908, 0x9FA1, 0x479F, 0x85, 0xBA, 0x99, 0x52, 0x21, 0x64, 0x47, 0xDB, 5);
        public static WindowsDevicePropertyKeys WPD_IMAGE_FNUMBER = new WindowsDevicePropertyKeys(0x63D64908, 0x9FA1, 0x479F, 0x85, 0xBA, 0x99, 0x52, 0x21, 0x64, 0x47, 0xDB, 6);
        public static WindowsDevicePropertyKeys WPD_IMAGE_EXPOSURE_TIME = new WindowsDevicePropertyKeys(0x63D64908, 0x9FA1, 0x479F, 0x85, 0xBA, 0x99, 0x52, 0x21, 0x64, 0x47, 0xDB, 7);
        public static WindowsDevicePropertyKeys WPD_IMAGE_EXPOSURE_INDEX = new WindowsDevicePropertyKeys(0x63D64908, 0x9FA1, 0x479F, 0x85, 0xBA, 0x99, 0x52, 0x21, 0x64, 0x47, 0xDB, 8);
        public static WindowsDevicePropertyKeys WPD_IMAGE_HORIZONTAL_RESOLUTION = new WindowsDevicePropertyKeys(0x63D64908, 0x9FA1, 0x479F, 0x85, 0xBA, 0x99, 0x52, 0x21, 0x64, 0x47, 0xDB, 9);
        public static WindowsDevicePropertyKeys WPD_IMAGE_VERTICAL_RESOLUTION = new WindowsDevicePropertyKeys(0x63D64908, 0x9FA1, 0x479F, 0x85, 0xBA, 0x99, 0x52, 0x21, 0x64, 0x47, 0xDB, 10);
        public static WindowsDevicePropertyKeys WPD_MEDIA_TOTAL_BITRATE = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 2);
        public static WindowsDevicePropertyKeys WPD_MEDIA_BITRATE_TYPE = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 3);
        public static WindowsDevicePropertyKeys WPD_MEDIA_COPYRIGHT = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 4);
        public static WindowsDevicePropertyKeys WPD_MEDIA_SUBSCRIPTION_CONTENT_ID = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 5);
        public static WindowsDevicePropertyKeys WPD_MEDIA_USE_COUNT = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 6);
        public static WindowsDevicePropertyKeys WPD_MEDIA_SKIP_COUNT = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 7);
        public static WindowsDevicePropertyKeys WPD_MEDIA_LAST_ACCESSED_TIME = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 8);
        public static WindowsDevicePropertyKeys WPD_MEDIA_PARENTAL_RATING = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 9);
        public static WindowsDevicePropertyKeys WPD_MEDIA_META_GENRE = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 10);
        public static WindowsDevicePropertyKeys WPD_MEDIA_COMPOSER = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 11);
        public static WindowsDevicePropertyKeys WPD_MEDIA_EFFECTIVE_RATING = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 12);
        public static WindowsDevicePropertyKeys WPD_MEDIA_SUB_TITLE = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 13);
        public static WindowsDevicePropertyKeys WPD_MEDIA_RELEASE_DATE = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 14);
        public static WindowsDevicePropertyKeys WPD_MEDIA_SAMPLE_RATE = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 15);
        public static WindowsDevicePropertyKeys WPD_MEDIA_STAR_RATING = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 16);
        public static WindowsDevicePropertyKeys WPD_MEDIA_USER_EFFECTIVE_RATING = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 17);
        public static WindowsDevicePropertyKeys WPD_MEDIA_TITLE = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 18);
        public static WindowsDevicePropertyKeys WPD_MEDIA_DURATION = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 19);
        public static WindowsDevicePropertyKeys WPD_MEDIA_BUY_NOW = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 20);
        public static WindowsDevicePropertyKeys WPD_MEDIA_ENCODING_PROFILE = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 21);
        public static WindowsDevicePropertyKeys WPD_MEDIA_WIDTH = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 22);
        public static WindowsDevicePropertyKeys WPD_MEDIA_HEIGHT = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 23);
        public static WindowsDevicePropertyKeys WPD_MEDIA_ARTIST = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 24);
        public static WindowsDevicePropertyKeys WPD_MEDIA_ALBUM_ARTIST = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 25);
        public static WindowsDevicePropertyKeys WPD_MEDIA_OWNER = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 26);
        public static WindowsDevicePropertyKeys WPD_MEDIA_MANAGING_EDITOR = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 27);
        public static WindowsDevicePropertyKeys WPD_MEDIA_WEBMASTER = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 28);
        public static WindowsDevicePropertyKeys WPD_MEDIA_SOURCE_URL = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 29);
        public static WindowsDevicePropertyKeys WPD_MEDIA_DESTINATION_URL = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 30);
        public static WindowsDevicePropertyKeys WPD_MEDIA_DESCRIPTION = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 31);
        public static WindowsDevicePropertyKeys WPD_MEDIA_GENRE = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 32);
        public static WindowsDevicePropertyKeys WPD_MEDIA_TIME_BOOKMARK = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 33);
        public static WindowsDevicePropertyKeys WPD_MEDIA_OBJECT_BOOKMARK = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 34);
        public static WindowsDevicePropertyKeys WPD_MEDIA_LAST_BUILD_DATE = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 35);
        public static WindowsDevicePropertyKeys WPD_MEDIA_BYTE_BOOKMARK = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 36);
        public static WindowsDevicePropertyKeys WPD_MEDIA_TIME_TO_LIVE = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 37);
        public static WindowsDevicePropertyKeys WPD_MEDIA_GUID = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 38);
        public static WindowsDevicePropertyKeys WPD_MEDIA_SUB_DESCRIPTION = new WindowsDevicePropertyKeys(0x2ED8BA05, 0x0AD3, 0x42DC, 0xB0, 0xD0, 0xBC, 0x95, 0xAC, 0x39, 0x6A, 0xC8, 39);
        public static WindowsDevicePropertyKeys WPD_CONTACT_DISPLAY_NAME = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 2);
        public static WindowsDevicePropertyKeys WPD_CONTACT_FIRST_NAME = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 3);
        public static WindowsDevicePropertyKeys WPD_CONTACT_MIDDLE_NAMES = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 4);
        public static WindowsDevicePropertyKeys WPD_CONTACT_LAST_NAME = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 5);
        public static WindowsDevicePropertyKeys WPD_CONTACT_PREFIX = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 6);
        public static WindowsDevicePropertyKeys WPD_CONTACT_SUFFIX = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 7);
        public static WindowsDevicePropertyKeys WPD_CONTACT_PHONETIC_FIRST_NAME = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 8);
        public static WindowsDevicePropertyKeys WPD_CONTACT_PHONETIC_LAST_NAME = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 9);
        public static WindowsDevicePropertyKeys WPD_CONTACT_PERSONAL_FULL_POSTAL_ADDRESS = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 10);
        public static WindowsDevicePropertyKeys WPD_CONTACT_PERSONAL_POSTAL_ADDRESS_LINE1 = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 11);
        public static WindowsDevicePropertyKeys WPD_CONTACT_PERSONAL_POSTAL_ADDRESS_LINE2 = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 12);
        public static WindowsDevicePropertyKeys WPD_CONTACT_PERSONAL_POSTAL_ADDRESS_CITY = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 13);
        public static WindowsDevicePropertyKeys WPD_CONTACT_PERSONAL_POSTAL_ADDRESS_REGION = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 14);
        public static WindowsDevicePropertyKeys WPD_CONTACT_PERSONAL_POSTAL_ADDRESS_POSTAL_CODE = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 15);
        public static WindowsDevicePropertyKeys WPD_CONTACT_PERSONAL_POSTAL_ADDRESS_COUNTRY = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 16);
        public static WindowsDevicePropertyKeys WPD_CONTACT_BUSINESS_FULL_POSTAL_ADDRESS = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 17);
        public static WindowsDevicePropertyKeys WPD_CONTACT_BUSINESS_POSTAL_ADDRESS_LINE1 = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 18);
        public static WindowsDevicePropertyKeys WPD_CONTACT_BUSINESS_POSTAL_ADDRESS_LINE2 = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 19);
        public static WindowsDevicePropertyKeys WPD_CONTACT_BUSINESS_POSTAL_ADDRESS_CITY = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 20);
        public static WindowsDevicePropertyKeys WPD_CONTACT_BUSINESS_POSTAL_ADDRESS_REGION = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 21);
        public static WindowsDevicePropertyKeys WPD_CONTACT_BUSINESS_POSTAL_ADDRESS_POSTAL_CODE = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 22);
        public static WindowsDevicePropertyKeys WPD_CONTACT_BUSINESS_POSTAL_ADDRESS_COUNTRY = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 23);
        public static WindowsDevicePropertyKeys WPD_CONTACT_OTHER_FULL_POSTAL_ADDRESS = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 24);
        public static WindowsDevicePropertyKeys WPD_CONTACT_OTHER_POSTAL_ADDRESS_LINE1 = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 25);
        public static WindowsDevicePropertyKeys WPD_CONTACT_OTHER_POSTAL_ADDRESS_LINE2 = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 26);
        public static WindowsDevicePropertyKeys WPD_CONTACT_OTHER_POSTAL_ADDRESS_CITY = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 27);
        public static WindowsDevicePropertyKeys WPD_CONTACT_OTHER_POSTAL_ADDRESS_REGION = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 28);
        public static WindowsDevicePropertyKeys WPD_CONTACT_OTHER_POSTAL_ADDRESS_POSTAL_CODE = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 29);
        public static WindowsDevicePropertyKeys WPD_CONTACT_OTHER_POSTAL_ADDRESS_POSTAL_COUNTRY = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 30);
        public static WindowsDevicePropertyKeys WPD_CONTACT_PRIMARY_EMAIL_ADDRESS = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 31);
        public static WindowsDevicePropertyKeys WPD_CONTACT_PERSONAL_EMAIL = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 32);
        public static WindowsDevicePropertyKeys WPD_CONTACT_PERSONAL_EMAIL2 = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 33);
        public static WindowsDevicePropertyKeys WPD_CONTACT_BUSINESS_EMAIL = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 34);
        public static WindowsDevicePropertyKeys WPD_CONTACT_BUSINESS_EMAIL2 = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 35);
        public static WindowsDevicePropertyKeys WPD_CONTACT_OTHER_EMAILS = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 36);
        public static WindowsDevicePropertyKeys WPD_CONTACT_PRIMARY_PHONE = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 37);
        public static WindowsDevicePropertyKeys WPD_CONTACT_PERSONAL_PHONE = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 38);
        public static WindowsDevicePropertyKeys WPD_CONTACT_PERSONAL_PHONE2 = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 39);
        public static WindowsDevicePropertyKeys WPD_CONTACT_BUSINESS_PHONE = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 40);
        public static WindowsDevicePropertyKeys WPD_CONTACT_BUSINESS_PHONE2 = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 41);
        public static WindowsDevicePropertyKeys WPD_CONTACT_MOBILE_PHONE = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 42);
        public static WindowsDevicePropertyKeys WPD_CONTACT_MOBILE_PHONE2 = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 43);
        public static WindowsDevicePropertyKeys WPD_CONTACT_PERSONAL_FAX = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 44);
        public static WindowsDevicePropertyKeys WPD_CONTACT_BUSINESS_FAX = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 45);
        public static WindowsDevicePropertyKeys WPD_CONTACT_PAGER = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 46);
        public static WindowsDevicePropertyKeys WPD_CONTACT_OTHER_PHONES = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 47);
        public static WindowsDevicePropertyKeys WPD_CONTACT_PRIMARY_WEB_ADDRESS = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 48);
        public static WindowsDevicePropertyKeys WPD_CONTACT_PERSONAL_WEB_ADDRESS = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 49);
        public static WindowsDevicePropertyKeys WPD_CONTACT_BUSINESS_WEB_ADDRESS = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 50);
        public static WindowsDevicePropertyKeys WPD_CONTACT_INSTANT_MESSENGER = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 51);
        public static WindowsDevicePropertyKeys WPD_CONTACT_INSTANT_MESSENGER2 = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 52);
        public static WindowsDevicePropertyKeys WPD_CONTACT_INSTANT_MESSENGER3 = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 53);
        public static WindowsDevicePropertyKeys WPD_CONTACT_COMPANY_NAME = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 54);
        public static WindowsDevicePropertyKeys WPD_CONTACT_PHONETIC_COMPANY_NAME = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 55);
        public static WindowsDevicePropertyKeys WPD_CONTACT_ROLE = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 56);
        public static WindowsDevicePropertyKeys WPD_CONTACT_BIRTHDATE = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 57);
        public static WindowsDevicePropertyKeys WPD_CONTACT_PRIMARY_FAX = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 58);
        public static WindowsDevicePropertyKeys WPD_CONTACT_SPOUSE = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 59);
        public static WindowsDevicePropertyKeys WPD_CONTACT_CHILDREN = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 60);
        public static WindowsDevicePropertyKeys WPD_CONTACT_ASSISTANT = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 61);
        public static WindowsDevicePropertyKeys WPD_CONTACT_ANNIVERSARY_DATE = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 62);
        public static WindowsDevicePropertyKeys WPD_CONTACT_RINGTONE = new WindowsDevicePropertyKeys(0xFBD4FDAB, 0x987D, 0x4777, 0xB3, 0xF9, 0x72, 0x61, 0x85, 0xA9, 0x31, 0x2B, 63);
        public static WindowsDevicePropertyKeys WPD_MUSIC_ALBUM = new WindowsDevicePropertyKeys(0xB324F56A, 0xDC5D, 0x46E5, 0xB6, 0xDF, 0xD2, 0xEA, 0x41, 0x48, 0x88, 0xC6, 3);
        public static WindowsDevicePropertyKeys WPD_MUSIC_TRACK = new WindowsDevicePropertyKeys(0xB324F56A, 0xDC5D, 0x46E5, 0xB6, 0xDF, 0xD2, 0xEA, 0x41, 0x48, 0x88, 0xC6, 4);
        public static WindowsDevicePropertyKeys WPD_MUSIC_LYRICS = new WindowsDevicePropertyKeys(0xB324F56A, 0xDC5D, 0x46E5, 0xB6, 0xDF, 0xD2, 0xEA, 0x41, 0x48, 0x88, 0xC6, 6);
        public static WindowsDevicePropertyKeys WPD_MUSIC_MOOD = new WindowsDevicePropertyKeys(0xB324F56A, 0xDC5D, 0x46E5, 0xB6, 0xDF, 0xD2, 0xEA, 0x41, 0x48, 0x88, 0xC6, 8);
        public static WindowsDevicePropertyKeys WPD_AUDIO_BITRATE = new WindowsDevicePropertyKeys(0xB324F56A, 0xDC5D, 0x46E5, 0xB6, 0xDF, 0xD2, 0xEA, 0x41, 0x48, 0x88, 0xC6, 9);
        public static WindowsDevicePropertyKeys WPD_AUDIO_CHANNEL_COUNT = new WindowsDevicePropertyKeys(0xB324F56A, 0xDC5D, 0x46E5, 0xB6, 0xDF, 0xD2, 0xEA, 0x41, 0x48, 0x88, 0xC6, 10);
        public static WindowsDevicePropertyKeys WPD_AUDIO_FORMAT_CODE = new WindowsDevicePropertyKeys(0xB324F56A, 0xDC5D, 0x46E5, 0xB6, 0xDF, 0xD2, 0xEA, 0x41, 0x48, 0x88, 0xC6, 11);
        public static WindowsDevicePropertyKeys WPD_AUDIO_BIT_DEPTH = new WindowsDevicePropertyKeys(0xB324F56A, 0xDC5D, 0x46E5, 0xB6, 0xDF, 0xD2, 0xEA, 0x41, 0x48, 0x88, 0xC6, 12);
        public static WindowsDevicePropertyKeys WPD_AUDIO_BLOCK_ALIGNMENT = new WindowsDevicePropertyKeys(0xB324F56A, 0xDC5D, 0x46E5, 0xB6, 0xDF, 0xD2, 0xEA, 0x41, 0x48, 0x88, 0xC6, 13);
        public static WindowsDevicePropertyKeys WPD_VIDEO_AUTHOR = new WindowsDevicePropertyKeys(0x346F2163, 0xF998, 0x4146, 0x8B, 0x01, 0xD1, 0x9B, 0x4C, 0x00, 0xDE, 0x9A, 2);
        public static WindowsDevicePropertyKeys WPD_VIDEO_RECORDEDTV_STATION_NAME = new WindowsDevicePropertyKeys(0x346F2163, 0xF998, 0x4146, 0x8B, 0x01, 0xD1, 0x9B, 0x4C, 0x00, 0xDE, 0x9A, 4);
        public static WindowsDevicePropertyKeys WPD_VIDEO_RECORDEDTV_CHANNEL_NUMBER = new WindowsDevicePropertyKeys(0x346F2163, 0xF998, 0x4146, 0x8B, 0x01, 0xD1, 0x9B, 0x4C, 0x00, 0xDE, 0x9A, 5);
        public static WindowsDevicePropertyKeys WPD_VIDEO_RECORDEDTV_REPEAT = new WindowsDevicePropertyKeys(0x346F2163, 0xF998, 0x4146, 0x8B, 0x01, 0xD1, 0x9B, 0x4C, 0x00, 0xDE, 0x9A, 7);
        public static WindowsDevicePropertyKeys WPD_VIDEO_BUFFER_SIZE = new WindowsDevicePropertyKeys(0x346F2163, 0xF998, 0x4146, 0x8B, 0x01, 0xD1, 0x9B, 0x4C, 0x00, 0xDE, 0x9A, 8);
        public static WindowsDevicePropertyKeys WPD_VIDEO_CREDITS = new WindowsDevicePropertyKeys(0x346F2163, 0xF998, 0x4146, 0x8B, 0x01, 0xD1, 0x9B, 0x4C, 0x00, 0xDE, 0x9A, 9);
        public static WindowsDevicePropertyKeys WPD_VIDEO_KEY_FRAME_DISTANCE = new WindowsDevicePropertyKeys(0x346F2163, 0xF998, 0x4146, 0x8B, 0x01, 0xD1, 0x9B, 0x4C, 0x00, 0xDE, 0x9A, 10);
        public static WindowsDevicePropertyKeys WPD_VIDEO_QUALITY_SETTING = new WindowsDevicePropertyKeys(0x346F2163, 0xF998, 0x4146, 0x8B, 0x01, 0xD1, 0x9B, 0x4C, 0x00, 0xDE, 0x9A, 11);
        public static WindowsDevicePropertyKeys WPD_VIDEO_SCAN_TYPE = new WindowsDevicePropertyKeys(0x346F2163, 0xF998, 0x4146, 0x8B, 0x01, 0xD1, 0x9B, 0x4C, 0x00, 0xDE, 0x9A, 12);
        public static WindowsDevicePropertyKeys WPD_VIDEO_BITRATE = new WindowsDevicePropertyKeys(0x346F2163, 0xF998, 0x4146, 0x8B, 0x01, 0xD1, 0x9B, 0x4C, 0x00, 0xDE, 0x9A, 13);
        public static WindowsDevicePropertyKeys WPD_VIDEO_FOURCC_CODE = new WindowsDevicePropertyKeys(0x346F2163, 0xF998, 0x4146, 0x8B, 0x01, 0xD1, 0x9B, 0x4C, 0x00, 0xDE, 0x9A, 14);
        public static WindowsDevicePropertyKeys WPD_VIDEO_FRAMERATE = new WindowsDevicePropertyKeys(0x346F2163, 0xF998, 0x4146, 0x8B, 0x01, 0xD1, 0x9B, 0x4C, 0x00, 0xDE, 0x9A, 15);
        public static WindowsDevicePropertyKeys WPD_COMMON_INFORMATION_SUBJECT = new WindowsDevicePropertyKeys(0xB28AE94B, 0x05A4, 0x4E8E, 0xBE, 0x01, 0x72, 0xCC, 0x7E, 0x09, 0x9D, 0x8F, 2);
        public static WindowsDevicePropertyKeys WPD_COMMON_INFORMATION_BODY_TEXT = new WindowsDevicePropertyKeys(0xB28AE94B, 0x05A4, 0x4E8E, 0xBE, 0x01, 0x72, 0xCC, 0x7E, 0x09, 0x9D, 0x8F, 3);
        public static WindowsDevicePropertyKeys WPD_COMMON_INFORMATION_PRIORITY = new WindowsDevicePropertyKeys(0xB28AE94B, 0x05A4, 0x4E8E, 0xBE, 0x01, 0x72, 0xCC, 0x7E, 0x09, 0x9D, 0x8F, 4);
        public static WindowsDevicePropertyKeys WPD_COMMON_INFORMATION_START_DATETIME = new WindowsDevicePropertyKeys(0xB28AE94B, 0x05A4, 0x4E8E, 0xBE, 0x01, 0x72, 0xCC, 0x7E, 0x09, 0x9D, 0x8F, 5);
        public static WindowsDevicePropertyKeys WPD_COMMON_INFORMATION_END_DATETIME = new WindowsDevicePropertyKeys(0xB28AE94B, 0x05A4, 0x4E8E, 0xBE, 0x01, 0x72, 0xCC, 0x7E, 0x09, 0x9D, 0x8F, 6);
        public static WindowsDevicePropertyKeys WPD_COMMON_INFORMATION_NOTES = new WindowsDevicePropertyKeys(0xB28AE94B, 0x05A4, 0x4E8E, 0xBE, 0x01, 0x72, 0xCC, 0x7E, 0x09, 0x9D, 0x8F, 7);
        public static WindowsDevicePropertyKeys WPD_EMAIL_TO_LINE = new WindowsDevicePropertyKeys(0x41F8F65A, 0x5484, 0x4782, 0xB1, 0x3D, 0x47, 0x40, 0xDD, 0x7C, 0x37, 0xC5, 2);
        public static WindowsDevicePropertyKeys WPD_EMAIL_CC_LINE = new WindowsDevicePropertyKeys(0x41F8F65A, 0x5484, 0x4782, 0xB1, 0x3D, 0x47, 0x40, 0xDD, 0x7C, 0x37, 0xC5, 3);
        public static WindowsDevicePropertyKeys WPD_EMAIL_BCC_LINE = new WindowsDevicePropertyKeys(0x41F8F65A, 0x5484, 0x4782, 0xB1, 0x3D, 0x47, 0x40, 0xDD, 0x7C, 0x37, 0xC5, 4);
        public static WindowsDevicePropertyKeys WPD_EMAIL_HAS_BEEN_READ = new WindowsDevicePropertyKeys(0x41F8F65A, 0x5484, 0x4782, 0xB1, 0x3D, 0x47, 0x40, 0xDD, 0x7C, 0x37, 0xC5, 7);
        public static WindowsDevicePropertyKeys WPD_EMAIL_RECEIVED_TIME = new WindowsDevicePropertyKeys(0x41F8F65A, 0x5484, 0x4782, 0xB1, 0x3D, 0x47, 0x40, 0xDD, 0x7C, 0x37, 0xC5, 8);
        public static WindowsDevicePropertyKeys WPD_EMAIL_HAS_ATTACHMENTS = new WindowsDevicePropertyKeys(0x41F8F65A, 0x5484, 0x4782, 0xB1, 0x3D, 0x47, 0x40, 0xDD, 0x7C, 0x37, 0xC5, 9);
        public static WindowsDevicePropertyKeys WPD_EMAIL_SENDER_ADDRESS = new WindowsDevicePropertyKeys(0x41F8F65A, 0x5484, 0x4782, 0xB1, 0x3D, 0x47, 0x40, 0xDD, 0x7C, 0x37, 0xC5, 10);
        public static WindowsDevicePropertyKeys WPD_APPOINTMENT_LOCATION = new WindowsDevicePropertyKeys(0xF99EFD03, 0x431D, 0x40D8, 0xA1, 0xC9, 0x4E, 0x22, 0x0D, 0x9C, 0x88, 0xD3, 3);
        public static WindowsDevicePropertyKeys WPD_APPOINTMENT_TYPE = new WindowsDevicePropertyKeys(0xF99EFD03, 0x431D, 0x40D8, 0xA1, 0xC9, 0x4E, 0x22, 0x0D, 0x9C, 0x88, 0xD3, 7);
        public static WindowsDevicePropertyKeys WPD_APPOINTMENT_REQUIRED_ATTENDEES = new WindowsDevicePropertyKeys(0xF99EFD03, 0x431D, 0x40D8, 0xA1, 0xC9, 0x4E, 0x22, 0x0D, 0x9C, 0x88, 0xD3, 8);
        public static WindowsDevicePropertyKeys WPD_APPOINTMENT_OPTIONAL_ATTENDEES = new WindowsDevicePropertyKeys(0xF99EFD03, 0x431D, 0x40D8, 0xA1, 0xC9, 0x4E, 0x22, 0x0D, 0x9C, 0x88, 0xD3, 9);
        public static WindowsDevicePropertyKeys WPD_APPOINTMENT_ACCEPTED_ATTENDEES = new WindowsDevicePropertyKeys(0xF99EFD03, 0x431D, 0x40D8, 0xA1, 0xC9, 0x4E, 0x22, 0x0D, 0x9C, 0x88, 0xD3, 10);
        public static WindowsDevicePropertyKeys WPD_APPOINTMENT_RESOURCES = new WindowsDevicePropertyKeys(0xF99EFD03, 0x431D, 0x40D8, 0xA1, 0xC9, 0x4E, 0x22, 0x0D, 0x9C, 0x88, 0xD3, 11);
        public static WindowsDevicePropertyKeys WPD_APPOINTMENT_TENTATIVE_ATTENDEES = new WindowsDevicePropertyKeys(0xF99EFD03, 0x431D, 0x40D8, 0xA1, 0xC9, 0x4E, 0x22, 0x0D, 0x9C, 0x88, 0xD3, 12);
        public static WindowsDevicePropertyKeys WPD_APPOINTMENT_DECLINED_ATTENDEES = new WindowsDevicePropertyKeys(0xF99EFD03, 0x431D, 0x40D8, 0xA1, 0xC9, 0x4E, 0x22, 0x0D, 0x9C, 0x88, 0xD3, 13);
        public static WindowsDevicePropertyKeys WPD_TASK_STATUS = new WindowsDevicePropertyKeys(0xE354E95E, 0xD8A0, 0x4637, 0xA0, 0x3A, 0x0C, 0xB2, 0x68, 0x38, 0xDB, 0xC7, 6);
        public static WindowsDevicePropertyKeys WPD_TASK_PERCENT_COMPLETE = new WindowsDevicePropertyKeys(0xE354E95E, 0xD8A0, 0x4637, 0xA0, 0x3A, 0x0C, 0xB2, 0x68, 0x38, 0xDB, 0xC7, 8);
        public static WindowsDevicePropertyKeys WPD_TASK_REMINDER_DATE = new WindowsDevicePropertyKeys(0xE354E95E, 0xD8A0, 0x4637, 0xA0, 0x3A, 0x0C, 0xB2, 0x68, 0x38, 0xDB, 0xC7, 10);
        public static WindowsDevicePropertyKeys WPD_TASK_OWNER = new WindowsDevicePropertyKeys(0xE354E95E, 0xD8A0, 0x4637, 0xA0, 0x3A, 0x0C, 0xB2, 0x68, 0x38, 0xDB, 0xC7, 11);
        public static WindowsDevicePropertyKeys WPD_NETWORK_ASSOCIATION_HOST_NETWORK_IDENTIFIERS = new WindowsDevicePropertyKeys(0xE4C93C1F, 0xB203, 0x43F1, 0xA1, 0x00, 0x5A, 0x07, 0xD1, 0x1B, 0x02, 0x74, 2);
        public static WindowsDevicePropertyKeys WPD_NETWORK_ASSOCIATION_X509V3SEQUENCE = new WindowsDevicePropertyKeys(0xE4C93C1F, 0xB203, 0x43F1, 0xA1, 0x00, 0x5A, 0x07, 0xD1, 0x1B, 0x02, 0x74, 3);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_CAPTURE_RESOLUTION = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 2);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_CAPTURE_FORMAT = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 3);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_COMPRESSION_SETTING = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 4);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_WHITE_BALANCE = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 5);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_RGB_GAIN = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 6);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_FNUMBER = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 7);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_FOCAL_LENGTH = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 8);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_FOCUS_DISTANCE = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 9);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_FOCUS_MODE = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 10);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_EXPOSURE_METERING_MODE = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 11);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_FLASH_MODE = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 12);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_EXPOSURE_TIME = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 13);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_EXPOSURE_PROGRAM_MODE = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 14);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_EXPOSURE_INDEX = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 15);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_EXPOSURE_BIAS_COMPENSATION = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 16);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_CAPTURE_DELAY = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 17);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_CAPTURE_MODE = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 18);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_CONTRAST = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 19);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_SHARPNESS = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 20);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_DIGITAL_ZOOM = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 21);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_EFFECT_MODE = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 22);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_BURST_NUMBER = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 23);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_BURST_INTERVAL = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 24);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_TIMELAPSE_NUMBER = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 25);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_TIMELAPSE_INTERVAL = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 26);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_FOCUS_METERING_MODE = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 27);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_UPLOAD_URL = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 28);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_ARTIST = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 29);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_CAMERA_MODEL = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 30);
        public static WindowsDevicePropertyKeys WPD_STILL_IMAGE_CAMERA_MANUFACTURER = new WindowsDevicePropertyKeys(0x58C571EC, 0x1BCB, 0x42A7, 0x8A, 0xC5, 0xBB, 0x29, 0x15, 0x73, 0xA2, 0x60, 31);
        public static WindowsDevicePropertyKeys WPD_SMS_PROVIDER = new WindowsDevicePropertyKeys(0x7E1074CC, 0x50FF, 0x4DD1, 0xA7, 0x42, 0x53, 0xBE, 0x6F, 0x09, 0x3A, 0x0D, 2);
        public static WindowsDevicePropertyKeys WPD_SMS_TIMEOUT = new WindowsDevicePropertyKeys(0x7E1074CC, 0x50FF, 0x4DD1, 0xA7, 0x42, 0x53, 0xBE, 0x6F, 0x09, 0x3A, 0x0D, 3);
        public static WindowsDevicePropertyKeys WPD_SMS_MAX_PAYLOAD = new WindowsDevicePropertyKeys(0x7E1074CC, 0x50FF, 0x4DD1, 0xA7, 0x42, 0x53, 0xBE, 0x6F, 0x09, 0x3A, 0x0D, 4);
        public static WindowsDevicePropertyKeys WPD_SMS_ENCODING = new WindowsDevicePropertyKeys(0x7E1074CC, 0x50FF, 0x4DD1, 0xA7, 0x42, 0x53, 0xBE, 0x6F, 0x09, 0x3A, 0x0D, 5);
        public static WindowsDevicePropertyKeys WPD_SECTION_DATA_OFFSET = new WindowsDevicePropertyKeys(0x516AFD2B, 0xC64E, 0x44F0, 0x98, 0xDC, 0xBE, 0xE1, 0xC8, 0x8F, 0x7D, 0x66, 2);
        public static WindowsDevicePropertyKeys WPD_SECTION_DATA_LENGTH = new WindowsDevicePropertyKeys(0x516AFD2B, 0xC64E, 0x44F0, 0x98, 0xDC, 0xBE, 0xE1, 0xC8, 0x8F, 0x7D, 0x66, 3);
        public static WindowsDevicePropertyKeys WPD_SECTION_DATA_UNITS = new WindowsDevicePropertyKeys(0x516AFD2B, 0xC64E, 0x44F0, 0x98, 0xDC, 0xBE, 0xE1, 0xC8, 0x8F, 0x7D, 0x66, 4);
        public static WindowsDevicePropertyKeys WPD_SECTION_DATA_REFERENCED_OBJECT_RESOURCE = new WindowsDevicePropertyKeys(0x516AFD2B, 0xC64E, 0x44F0, 0x98, 0xDC, 0xBE, 0xE1, 0xC8, 0x8F, 0x7D, 0x66, 5);
        public static WindowsDevicePropertyKeys WPD_RENDERING_INFORMATION_PROFILES = new WindowsDevicePropertyKeys(0xC53D039F, 0xEE23, 0x4A31, 0x85, 0x90, 0x76, 0x39, 0x87, 0x98, 0x70, 0xB4, 2);
        public static WindowsDevicePropertyKeys WPD_RENDERING_INFORMATION_PROFILE_ENTRY_TYPE = new WindowsDevicePropertyKeys(0xC53D039F, 0xEE23, 0x4A31, 0x85, 0x90, 0x76, 0x39, 0x87, 0x98, 0x70, 0xB4, 3);
        public static WindowsDevicePropertyKeys WPD_RENDERING_INFORMATION_PROFILE_ENTRY_CREATABLE_RESOURCES = new WindowsDevicePropertyKeys(0xC53D039F, 0xEE23, 0x4A31, 0x85, 0x90, 0x76, 0x39, 0x87, 0x98, 0x70, 0xB4, 4);
        public static WindowsDevicePropertyKeys WPD_COMMAND_STORAGE_FORMAT = new WindowsDevicePropertyKeys(0xD8F907A6, 0x34CC, 0x45FA, 0x97, 0xFB, 0xD0, 0x07, 0xFA, 0x47, 0xEC, 0x94, 2);
        public static WindowsDevicePropertyKeys WPD_COMMAND_STORAGE_EJECT = new WindowsDevicePropertyKeys(0xD8F907A6, 0x34CC, 0x45FA, 0x97, 0xFB, 0xD0, 0x07, 0xFA, 0x47, 0xEC, 0x94, 4);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_STORAGE_OBJECT_ID = new WindowsDevicePropertyKeys(0xD8F907A6, 0x34CC, 0x45FA, 0x97, 0xFB, 0xD0, 0x07, 0xFA, 0x47, 0xEC, 0x94, 1001);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_STORAGE_DESTINATION_OBJECT_ID = new WindowsDevicePropertyKeys(0xD8F907A6, 0x34CC, 0x45FA, 0x97, 0xFB, 0xD0, 0x07, 0xFA, 0x47, 0xEC, 0x94, 1002);
        public static WindowsDevicePropertyKeys WPD_COMMAND_SMS_SEND = new WindowsDevicePropertyKeys(0xAFC25D66, 0xFE0D, 0x4114, 0x90, 0x97, 0x97, 0x0C, 0x93, 0xE9, 0x20, 0xD1, 2);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_SMS_RECIPIENT = new WindowsDevicePropertyKeys(0xAFC25D66, 0xFE0D, 0x4114, 0x90, 0x97, 0x97, 0x0C, 0x93, 0xE9, 0x20, 0xD1, 1001);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_SMS_MESSAGE_TYPE = new WindowsDevicePropertyKeys(0xAFC25D66, 0xFE0D, 0x4114, 0x90, 0x97, 0x97, 0x0C, 0x93, 0xE9, 0x20, 0xD1, 1002);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_SMS_TEXT_MESSAGE = new WindowsDevicePropertyKeys(0xAFC25D66, 0xFE0D, 0x4114, 0x90, 0x97, 0x97, 0x0C, 0x93, 0xE9, 0x20, 0xD1, 1003);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_SMS_BINARY_MESSAGE = new WindowsDevicePropertyKeys(0xAFC25D66, 0xFE0D, 0x4114, 0x90, 0x97, 0x97, 0x0C, 0x93, 0xE9, 0x20, 0xD1, 1004);
        public static WindowsDevicePropertyKeys WPD_OPTION_SMS_BINARY_MESSAGE_SUPPORTED = new WindowsDevicePropertyKeys(0xAFC25D66, 0xFE0D, 0x4114, 0x90, 0x97, 0x97, 0x0C, 0x93, 0xE9, 0x20, 0xD1, 5001);
        public static WindowsDevicePropertyKeys WPD_COMMAND_STILL_IMAGE_CAPTURE_INITIATE = new WindowsDevicePropertyKeys(0x4FCD6982, 0x22A2, 0x4B05, 0xA4, 0x8B, 0x62, 0xD3, 0x8B, 0xF2, 0x7B, 0x32, 2);
        public static WindowsDevicePropertyKeys WPD_COMMAND_MEDIA_CAPTURE_START = new WindowsDevicePropertyKeys(0x59B433BA, 0xFE44, 0x4D8D, 0x80, 0x8C, 0x6B, 0xCB, 0x9B, 0x0F, 0x15, 0xE8, 2);
        public static WindowsDevicePropertyKeys WPD_COMMAND_MEDIA_CAPTURE_STOP = new WindowsDevicePropertyKeys(0x59B433BA, 0xFE44, 0x4D8D, 0x80, 0x8C, 0x6B, 0xCB, 0x9B, 0x0F, 0x15, 0xE8, 3);
        public static WindowsDevicePropertyKeys WPD_COMMAND_MEDIA_CAPTURE_PAUSE = new WindowsDevicePropertyKeys(0x59B433BA, 0xFE44, 0x4D8D, 0x80, 0x8C, 0x6B, 0xCB, 0x9B, 0x0F, 0x15, 0xE8, 4);
        public static WindowsDevicePropertyKeys WPD_COMMAND_DEVICE_HINTS_GET_CONTENT_LOCATION = new WindowsDevicePropertyKeys(0x0D5FB92B, 0xCB46, 0x4C4F, 0x83, 0x43, 0x0B, 0xC3, 0xD3, 0xF1, 0x7C, 0x84, 2);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_DEVICE_HINTS_CONTENT_TYPE = new WindowsDevicePropertyKeys(0x0D5FB92B, 0xCB46, 0x4C4F, 0x83, 0x43, 0x0B, 0xC3, 0xD3, 0xF1, 0x7C, 0x84, 1001);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_DEVICE_HINTS_CONTENT_LOCATIONS = new WindowsDevicePropertyKeys(0x0D5FB92B, 0xCB46, 0x4C4F, 0x83, 0x43, 0x0B, 0xC3, 0xD3, 0xF1, 0x7C, 0x84, 1002);
        public static WindowsDevicePropertyKeys WPD_COMMAND_GENERATE_KEYPAIR = new WindowsDevicePropertyKeys(0x78F9C6FC, 0x79B8, 0x473C, 0x90, 0x60, 0x6B, 0xD2, 0x3D, 0xD0, 0x72, 0xC4, 2);
        public static WindowsDevicePropertyKeys WPD_COMMAND_COMMIT_KEYPAIR = new WindowsDevicePropertyKeys(0x78F9C6FC, 0x79B8, 0x473C, 0x90, 0x60, 0x6B, 0xD2, 0x3D, 0xD0, 0x72, 0xC4, 3);
        public static WindowsDevicePropertyKeys WPD_COMMAND_PROCESS_WIRELESS_PROFILE = new WindowsDevicePropertyKeys(0x78F9C6FC, 0x79B8, 0x473C, 0x90, 0x60, 0x6B, 0xD2, 0x3D, 0xD0, 0x72, 0xC4, 4);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_PUBLIC_KEY = new WindowsDevicePropertyKeys(0x78F9C6FC, 0x79B8, 0x473C, 0x90, 0x60, 0x6B, 0xD2, 0x3D, 0xD0, 0x72, 0xC4, 1001);
        public static WindowsDevicePropertyKeys WPD_RESOURCE_DEFAULT = new WindowsDevicePropertyKeys(0xE81E79BE, 0x34F0, 0x41BF, 0xB5, 0x3F, 0xF1, 0xA0, 0x6A, 0xE8, 0x78, 0x42, 0);
        public static WindowsDevicePropertyKeys WPD_RESOURCE_CONTACT_PHOTO = new WindowsDevicePropertyKeys(0x2C4D6803, 0x80EA, 0x4580, 0xAF, 0x9A, 0x5B, 0xE1, 0xA2, 0x3E, 0xDD, 0xCB, 0);
        public static WindowsDevicePropertyKeys WPD_RESOURCE_THUMBNAIL = new WindowsDevicePropertyKeys(0xC7C407BA, 0x98FA, 0x46B5, 0x99, 0x60, 0x23, 0xFE, 0xC1, 0x24, 0xCF, 0xDE, 0);
        public static WindowsDevicePropertyKeys WPD_RESOURCE_ICON = new WindowsDevicePropertyKeys(0xF195FED8, 0xAA28, 0x4EE3, 0xB1, 0x53, 0xE1, 0x82, 0xDD, 0x5E, 0xDC, 0x39, 0);
        public static WindowsDevicePropertyKeys WPD_RESOURCE_AUDIO_CLIP = new WindowsDevicePropertyKeys(0x3BC13982, 0x85B1, 0x48E0, 0x95, 0xA6, 0x8D, 0x3A, 0xD0, 0x6B, 0xE1, 0x17, 0);
        public static WindowsDevicePropertyKeys WPD_RESOURCE_ALBUM_ART = new WindowsDevicePropertyKeys(0xF02AA354, 0x2300, 0x4E2D, 0xA1, 0xB9, 0x3B, 0x67, 0x30, 0xF7, 0xFA, 0x21, 0);
        public static WindowsDevicePropertyKeys WPD_RESOURCE_GENERIC = new WindowsDevicePropertyKeys(0xB9B9F515, 0xBA70, 0x4647, 0x94, 0xDC, 0xFA, 0x49, 0x25, 0xE9, 0x5A, 0x07, 0);
        public static WindowsDevicePropertyKeys WPD_RESOURCE_VIDEO_CLIP = new WindowsDevicePropertyKeys(0xB566EE42, 0x6368, 0x4290, 0x86, 0x62, 0x70, 0x18, 0x2F, 0xB7, 0x9F, 0x20, 0);
        public static WindowsDevicePropertyKeys WPD_RESOURCE_BRANDING_ART = new WindowsDevicePropertyKeys(0xB633B1AE, 0x6CAF, 0x4A87, 0x95, 0x89, 0x22, 0xDE, 0xD6, 0xDD, 0x58, 0x99, 0);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_NULL = new WindowsDevicePropertyKeys(0x00000000, 0x0000, 0x0000, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0);
        public static WindowsDevicePropertyKeys WPD_FUNCTIONAL_OBJECT_CATEGORY = new WindowsDevicePropertyKeys(0x8F052D93, 0xABCA, 0x4FC5, 0xA5, 0xAC, 0xB0, 0x1D, 0xF4, 0xDB, 0xE5, 0x98, 2);
        public static WindowsDevicePropertyKeys WPD_STORAGE_TYPE = new WindowsDevicePropertyKeys(0x01A3057A, 0x74D6, 0x4E80, 0xBE, 0xA7, 0xDC, 0x4C, 0x21, 0x2C, 0xE5, 0x0A, 2);
        public static WindowsDevicePropertyKeys WPD_STORAGE_FILE_SYSTEM_TYPE = new WindowsDevicePropertyKeys(0x01A3057A, 0x74D6, 0x4E80, 0xBE, 0xA7, 0xDC, 0x4C, 0x21, 0x2C, 0xE5, 0x0A, 3);
        public static WindowsDevicePropertyKeys WPD_STORAGE_CAPACITY = new WindowsDevicePropertyKeys(0x01A3057A, 0x74D6, 0x4E80, 0xBE, 0xA7, 0xDC, 0x4C, 0x21, 0x2C, 0xE5, 0x0A, 4);
        public static WindowsDevicePropertyKeys WPD_STORAGE_FREE_SPACE_IN_BYTES = new WindowsDevicePropertyKeys(0x01A3057A, 0x74D6, 0x4E80, 0xBE, 0xA7, 0xDC, 0x4C, 0x21, 0x2C, 0xE5, 0x0A, 5);
        public static WindowsDevicePropertyKeys WPD_STORAGE_FREE_SPACE_IN_OBJECTS = new WindowsDevicePropertyKeys(0x01A3057A, 0x74D6, 0x4E80, 0xBE, 0xA7, 0xDC, 0x4C, 0x21, 0x2C, 0xE5, 0x0A, 6);
        public static WindowsDevicePropertyKeys WPD_STORAGE_DESCRIPTION = new WindowsDevicePropertyKeys(0x01A3057A, 0x74D6, 0x4E80, 0xBE, 0xA7, 0xDC, 0x4C, 0x21, 0x2C, 0xE5, 0x0A, 7);
        public static WindowsDevicePropertyKeys WPD_STORAGE_SERIAL_NUMBER = new WindowsDevicePropertyKeys(0x01A3057A, 0x74D6, 0x4E80, 0xBE, 0xA7, 0xDC, 0x4C, 0x21, 0x2C, 0xE5, 0x0A, 8);
        public static WindowsDevicePropertyKeys WPD_STORAGE_MAX_OBJECT_SIZE = new WindowsDevicePropertyKeys(0x01A3057A, 0x74D6, 0x4E80, 0xBE, 0xA7, 0xDC, 0x4C, 0x21, 0x2C, 0xE5, 0x0A, 9);
        public static WindowsDevicePropertyKeys WPD_STORAGE_CAPACITY_IN_OBJECTS = new WindowsDevicePropertyKeys(0x01A3057A, 0x74D6, 0x4E80, 0xBE, 0xA7, 0xDC, 0x4C, 0x21, 0x2C, 0xE5, 0x0A, 10);
        public static WindowsDevicePropertyKeys WPD_STORAGE_ACCESS_CAPABILITY = new WindowsDevicePropertyKeys(0x01A3057A, 0x74D6, 0x4E80, 0xBE, 0xA7, 0xDC, 0x4C, 0x21, 0x2C, 0xE5, 0x0A, 11);
        public static WindowsDevicePropertyKeys WPD_CLIENT_NAME = new WindowsDevicePropertyKeys(0x204D9F0C, 0x2292, 0x4080, 0x9F, 0x42, 0x40, 0x66, 0x4E, 0x70, 0xF8, 0x59, 2);
        public static WindowsDevicePropertyKeys WPD_CLIENT_MAJOR_VERSION = new WindowsDevicePropertyKeys(0x204D9F0C, 0x2292, 0x4080, 0x9F, 0x42, 0x40, 0x66, 0x4E, 0x70, 0xF8, 0x59, 3);
        public static WindowsDevicePropertyKeys WPD_CLIENT_MINOR_VERSION = new WindowsDevicePropertyKeys(0x204D9F0C, 0x2292, 0x4080, 0x9F, 0x42, 0x40, 0x66, 0x4E, 0x70, 0xF8, 0x59, 4);
        public static WindowsDevicePropertyKeys WPD_CLIENT_REVISION = new WindowsDevicePropertyKeys(0x204D9F0C, 0x2292, 0x4080, 0x9F, 0x42, 0x40, 0x66, 0x4E, 0x70, 0xF8, 0x59, 5);
        public static WindowsDevicePropertyKeys WPD_CLIENT_WMDRM_APPLICATION_PRIVATE_KEY = new WindowsDevicePropertyKeys(0x204D9F0C, 0x2292, 0x4080, 0x9F, 0x42, 0x40, 0x66, 0x4E, 0x70, 0xF8, 0x59, 6);
        public static WindowsDevicePropertyKeys WPD_CLIENT_WMDRM_APPLICATION_CERTIFICATE = new WindowsDevicePropertyKeys(0x204D9F0C, 0x2292, 0x4080, 0x9F, 0x42, 0x40, 0x66, 0x4E, 0x70, 0xF8, 0x59, 7);
        public static WindowsDevicePropertyKeys WPD_CLIENT_SECURITY_QUALITY_OF_SERVICE = new WindowsDevicePropertyKeys(0x204D9F0C, 0x2292, 0x4080, 0x9F, 0x42, 0x40, 0x66, 0x4E, 0x70, 0xF8, 0x59, 8);
        public static WindowsDevicePropertyKeys WPD_CLIENT_DESIRED_ACCESS = new WindowsDevicePropertyKeys(0x204D9F0C, 0x2292, 0x4080, 0x9F, 0x42, 0x40, 0x66, 0x4E, 0x70, 0xF8, 0x59, 9);
        public static WindowsDevicePropertyKeys WPD_CLIENT_SHARE_MODE = new WindowsDevicePropertyKeys(0x204D9F0C, 0x2292, 0x4080, 0x9F, 0x42, 0x40, 0x66, 0x4E, 0x70, 0xF8, 0x59, 10);
        public static WindowsDevicePropertyKeys WPD_CLIENT_EVENT_COOKIE = new WindowsDevicePropertyKeys(0x204D9F0C, 0x2292, 0x4080, 0x9F, 0x42, 0x40, 0x66, 0x4E, 0x70, 0xF8, 0x59, 11);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_ATTRIBUTE_FORM = new WindowsDevicePropertyKeys(0xAB7943D8, 0x6332, 0x445F, 0xA0, 0x0D, 0x8D, 0x5E, 0xF1, 0xE9, 0x6F, 0x37, 2);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_ATTRIBUTE_CAN_READ = new WindowsDevicePropertyKeys(0xAB7943D8, 0x6332, 0x445F, 0xA0, 0x0D, 0x8D, 0x5E, 0xF1, 0xE9, 0x6F, 0x37, 3);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_ATTRIBUTE_CAN_WRITE = new WindowsDevicePropertyKeys(0xAB7943D8, 0x6332, 0x445F, 0xA0, 0x0D, 0x8D, 0x5E, 0xF1, 0xE9, 0x6F, 0x37, 4);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_ATTRIBUTE_CAN_DELETE = new WindowsDevicePropertyKeys(0xAB7943D8, 0x6332, 0x445F, 0xA0, 0x0D, 0x8D, 0x5E, 0xF1, 0xE9, 0x6F, 0x37, 5);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_ATTRIBUTE_DEFAULT_VALUE = new WindowsDevicePropertyKeys(0xAB7943D8, 0x6332, 0x445F, 0xA0, 0x0D, 0x8D, 0x5E, 0xF1, 0xE9, 0x6F, 0x37, 6);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_ATTRIBUTE_FAST_PROPERTY = new WindowsDevicePropertyKeys(0xAB7943D8, 0x6332, 0x445F, 0xA0, 0x0D, 0x8D, 0x5E, 0xF1, 0xE9, 0x6F, 0x37, 7);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_ATTRIBUTE_RANGE_MIN = new WindowsDevicePropertyKeys(0xAB7943D8, 0x6332, 0x445F, 0xA0, 0x0D, 0x8D, 0x5E, 0xF1, 0xE9, 0x6F, 0x37, 8);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_ATTRIBUTE_RANGE_MAX = new WindowsDevicePropertyKeys(0xAB7943D8, 0x6332, 0x445F, 0xA0, 0x0D, 0x8D, 0x5E, 0xF1, 0xE9, 0x6F, 0x37, 9);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_ATTRIBUTE_RANGE_STEP = new WindowsDevicePropertyKeys(0xAB7943D8, 0x6332, 0x445F, 0xA0, 0x0D, 0x8D, 0x5E, 0xF1, 0xE9, 0x6F, 0x37, 10);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_ATTRIBUTE_ENUMERATION_ELEMENTS = new WindowsDevicePropertyKeys(0xAB7943D8, 0x6332, 0x445F, 0xA0, 0x0D, 0x8D, 0x5E, 0xF1, 0xE9, 0x6F, 0x37, 11);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_ATTRIBUTE_REGULAR_EXPRESSION = new WindowsDevicePropertyKeys(0xAB7943D8, 0x6332, 0x445F, 0xA0, 0x0D, 0x8D, 0x5E, 0xF1, 0xE9, 0x6F, 0x37, 12);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_ATTRIBUTE_MAX_SIZE = new WindowsDevicePropertyKeys(0xAB7943D8, 0x6332, 0x445F, 0xA0, 0x0D, 0x8D, 0x5E, 0xF1, 0xE9, 0x6F, 0x37, 13);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_ATTRIBUTE_NAME = new WindowsDevicePropertyKeys(0x5D9DA160, 0x74AE, 0x43CC, 0x85, 0xA9, 0xFE, 0x55, 0x5A, 0x80, 0x79, 0x8E, 2);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_ATTRIBUTE_VARTYPE = new WindowsDevicePropertyKeys(0x5D9DA160, 0x74AE, 0x43CC, 0x85, 0xA9, 0xFE, 0x55, 0x5A, 0x80, 0x79, 0x8E, 3);
        public static WindowsDevicePropertyKeys WPD_CLASS_EXTENSION_OPTIONS_SUPPORTED_CONTENT_TYPES = new WindowsDevicePropertyKeys(0x6309FFEF, 0xA87C, 0x4CA7, 0x84, 0x34, 0x79, 0x75, 0x76, 0xE4, 0x0A, 0x96, 2);
        public static WindowsDevicePropertyKeys WPD_CLASS_EXTENSION_OPTIONS_DONT_REGISTER_WPD_DEVICE_INTERFACE = new WindowsDevicePropertyKeys(0x6309FFEF, 0xA87C, 0x4CA7, 0x84, 0x34, 0x79, 0x75, 0x76, 0xE4, 0x0A, 0x96, 3);
        public static WindowsDevicePropertyKeys WPD_CLASS_EXTENSION_OPTIONS_REGISTER_WPD_PRIVATE_DEVICE_INTERFACE = new WindowsDevicePropertyKeys(0x6309FFEF, 0xA87C, 0x4CA7, 0x84, 0x34, 0x79, 0x75, 0x76, 0xE4, 0x0A, 0x96, 4);
        public static WindowsDevicePropertyKeys WPD_CLASS_EXTENSION_OPTIONS_MULTITRANSPORT_MODE = new WindowsDevicePropertyKeys(0X3E3595DA, 0X4D71, 0X49FE, 0XA0, 0XB4, 0XD4, 0X40, 0X6C, 0X3A, 0XE9, 0X3F, 2);
        public static WindowsDevicePropertyKeys WPD_CLASS_EXTENSION_OPTIONS_DEVICE_IDENTIFICATION_VALUES = new WindowsDevicePropertyKeys(0X3E3595DA, 0X4D71, 0X49FE, 0XA0, 0XB4, 0XD4, 0X40, 0X6C, 0X3A, 0XE9, 0X3F, 3);
        public static WindowsDevicePropertyKeys WPD_CLASS_EXTENSION_OPTIONS_TRANSPORT_BANDWIDTH = new WindowsDevicePropertyKeys(0X3E3595DA, 0X4D71, 0X49FE, 0XA0, 0XB4, 0XD4, 0X40, 0X6C, 0X3A, 0XE9, 0X3F, 4);
        public static WindowsDevicePropertyKeys WPD_RESOURCE_ATTRIBUTE_TOTAL_SIZE = new WindowsDevicePropertyKeys(0x1EB6F604, 0x9278, 0x429F, 0x93, 0xCC, 0x5B, 0xB8, 0xC0, 0x66, 0x56, 0xB6, 2);
        public static WindowsDevicePropertyKeys WPD_RESOURCE_ATTRIBUTE_CAN_READ = new WindowsDevicePropertyKeys(0x1EB6F604, 0x9278, 0x429F, 0x93, 0xCC, 0x5B, 0xB8, 0xC0, 0x66, 0x56, 0xB6, 3);
        public static WindowsDevicePropertyKeys WPD_RESOURCE_ATTRIBUTE_CAN_WRITE = new WindowsDevicePropertyKeys(0x1EB6F604, 0x9278, 0x429F, 0x93, 0xCC, 0x5B, 0xB8, 0xC0, 0x66, 0x56, 0xB6, 4);
        public static WindowsDevicePropertyKeys WPD_RESOURCE_ATTRIBUTE_CAN_DELETE = new WindowsDevicePropertyKeys(0x1EB6F604, 0x9278, 0x429F, 0x93, 0xCC, 0x5B, 0xB8, 0xC0, 0x66, 0x56, 0xB6, 5);
        public static WindowsDevicePropertyKeys WPD_RESOURCE_ATTRIBUTE_OPTIMAL_READ_BUFFER_SIZE = new WindowsDevicePropertyKeys(0x1EB6F604, 0x9278, 0x429F, 0x93, 0xCC, 0x5B, 0xB8, 0xC0, 0x66, 0x56, 0xB6, 6);
        public static WindowsDevicePropertyKeys WPD_RESOURCE_ATTRIBUTE_OPTIMAL_WRITE_BUFFER_SIZE = new WindowsDevicePropertyKeys(0x1EB6F604, 0x9278, 0x429F, 0x93, 0xCC, 0x5B, 0xB8, 0xC0, 0x66, 0x56, 0xB6, 7);
        public static WindowsDevicePropertyKeys WPD_RESOURCE_ATTRIBUTE_FORMAT = new WindowsDevicePropertyKeys(0x1EB6F604, 0x9278, 0x429F, 0x93, 0xCC, 0x5B, 0xB8, 0xC0, 0x66, 0x56, 0xB6, 8);
        public static WindowsDevicePropertyKeys WPD_RESOURCE_ATTRIBUTE_RESOURCE_KEY = new WindowsDevicePropertyKeys(0x1EB6F604, 0x9278, 0x429F, 0x93, 0xCC, 0x5B, 0xB8, 0xC0, 0x66, 0x56, 0xB6, 9);
        public static WindowsDevicePropertyKeys WPD_DEVICE_SYNC_PARTNER = new WindowsDevicePropertyKeys(0x26D4979A, 0xE643, 0x4626, 0x9E, 0x2B, 0x73, 0x6D, 0xC0, 0xC9, 0x2F, 0xDC, 2);
        public static WindowsDevicePropertyKeys WPD_DEVICE_FIRMWARE_VERSION = new WindowsDevicePropertyKeys(0x26D4979A, 0xE643, 0x4626, 0x9E, 0x2B, 0x73, 0x6D, 0xC0, 0xC9, 0x2F, 0xDC, 3);
        public static WindowsDevicePropertyKeys WPD_DEVICE_POWER_LEVEL = new WindowsDevicePropertyKeys(0x26D4979A, 0xE643, 0x4626, 0x9E, 0x2B, 0x73, 0x6D, 0xC0, 0xC9, 0x2F, 0xDC, 4);
        public static WindowsDevicePropertyKeys WPD_DEVICE_POWER_SOURCE = new WindowsDevicePropertyKeys(0x26D4979A, 0xE643, 0x4626, 0x9E, 0x2B, 0x73, 0x6D, 0xC0, 0xC9, 0x2F, 0xDC, 5);
        public static WindowsDevicePropertyKeys WPD_DEVICE_PROTOCOL = new WindowsDevicePropertyKeys(0x26D4979A, 0xE643, 0x4626, 0x9E, 0x2B, 0x73, 0x6D, 0xC0, 0xC9, 0x2F, 0xDC, 6);
        public static WindowsDevicePropertyKeys WPD_DEVICE_MANUFACTURER = new WindowsDevicePropertyKeys(0x26D4979A, 0xE643, 0x4626, 0x9E, 0x2B, 0x73, 0x6D, 0xC0, 0xC9, 0x2F, 0xDC, 7);
        public static WindowsDevicePropertyKeys WPD_DEVICE_MODEL = new WindowsDevicePropertyKeys(0x26D4979A, 0xE643, 0x4626, 0x9E, 0x2B, 0x73, 0x6D, 0xC0, 0xC9, 0x2F, 0xDC, 8);
        public static WindowsDevicePropertyKeys WPD_DEVICE_SERIAL_NUMBER = new WindowsDevicePropertyKeys(0x26D4979A, 0xE643, 0x4626, 0x9E, 0x2B, 0x73, 0x6D, 0xC0, 0xC9, 0x2F, 0xDC, 9);
        public static WindowsDevicePropertyKeys WPD_DEVICE_SUPPORTS_NON_CONSUMABLE = new WindowsDevicePropertyKeys(0x26D4979A, 0xE643, 0x4626, 0x9E, 0x2B, 0x73, 0x6D, 0xC0, 0xC9, 0x2F, 0xDC, 10);
        public static WindowsDevicePropertyKeys WPD_DEVICE_DATETIME = new WindowsDevicePropertyKeys(0x26D4979A, 0xE643, 0x4626, 0x9E, 0x2B, 0x73, 0x6D, 0xC0, 0xC9, 0x2F, 0xDC, 11);
        public static WindowsDevicePropertyKeys WPD_DEVICE_FRIENDLY_NAME = new WindowsDevicePropertyKeys(0x26D4979A, 0xE643, 0x4626, 0x9E, 0x2B, 0x73, 0x6D, 0xC0, 0xC9, 0x2F, 0xDC, 12);
        public static WindowsDevicePropertyKeys WPD_DEVICE_SUPPORTED_DRM_SCHEMES = new WindowsDevicePropertyKeys(0x26D4979A, 0xE643, 0x4626, 0x9E, 0x2B, 0x73, 0x6D, 0xC0, 0xC9, 0x2F, 0xDC, 13);
        public static WindowsDevicePropertyKeys WPD_DEVICE_SUPPORTED_FORMATS_ARE_ORDERED = new WindowsDevicePropertyKeys(0x26D4979A, 0xE643, 0x4626, 0x9E, 0x2B, 0x73, 0x6D, 0xC0, 0xC9, 0x2F, 0xDC, 14);
        public static WindowsDevicePropertyKeys WPD_DEVICE_TYPE = new WindowsDevicePropertyKeys(0x26D4979A, 0xE643, 0x4626, 0x9E, 0x2B, 0x73, 0x6D, 0xC0, 0xC9, 0x2F, 0xDC, 15);
        public static WindowsDevicePropertyKeys WPD_DEVICE_NETWORK_IDENTIFIER = new WindowsDevicePropertyKeys(0x26D4979A, 0xE643, 0x4626, 0x9E, 0x2B, 0x73, 0x6D, 0xC0, 0xC9, 0x2F, 0xDC, 16);
        public static WindowsDevicePropertyKeys WPD_DEVICE_FUNCTIONAL_UNIQUE_ID = new WindowsDevicePropertyKeys(0x463DD662, 0x7FC4, 0x4291, 0x91, 0x1C, 0x7F, 0x4C, 0x9C, 0xCA, 0x97, 0x99, 2);
        public static WindowsDevicePropertyKeys WPD_DEVICE_MODEL_UNIQUE_ID = new WindowsDevicePropertyKeys(0x463DD662, 0x7FC4, 0x4291, 0x91, 0x1C, 0x7F, 0x4C, 0x9C, 0xCA, 0x97, 0x99, 3);
        public static WindowsDevicePropertyKeys WPD_DEVICE_TRANSPORT = new WindowsDevicePropertyKeys(0x463DD662, 0x7FC4, 0x4291, 0x91, 0x1C, 0x7F, 0x4C, 0x9C, 0xCA, 0x97, 0x99, 4);
        public static WindowsDevicePropertyKeys WPD_DEVICE_USE_DEVICE_STAGE = new WindowsDevicePropertyKeys(0x463DD662, 0x7FC4, 0x4291, 0x91, 0x1C, 0x7F, 0x4C, 0x9C, 0xCA, 0x97, 0x99, 5);
        public static WindowsDevicePropertyKeys WPD_SERVICE_VERSION = new WindowsDevicePropertyKeys(0x7510698A, 0xCB54, 0x481C, 0xB8, 0xDB, 0x0D, 0x75, 0xC9, 0x3F, 0x1C, 0x06, 2);
        public static WindowsDevicePropertyKeys WPD_EVENT_PARAMETER_PNP_DEVICE_ID = new WindowsDevicePropertyKeys(0x15AB1953, 0xF817, 0x4FEF, 0xA9, 0x21, 0x56, 0x76, 0xE8, 0x38, 0xF6, 0xE0, 2);
        public static WindowsDevicePropertyKeys WPD_EVENT_PARAMETER_EVENT_ID = new WindowsDevicePropertyKeys(0x15AB1953, 0xF817, 0x4FEF, 0xA9, 0x21, 0x56, 0x76, 0xE8, 0x38, 0xF6, 0xE0, 3);
        public static WindowsDevicePropertyKeys WPD_EVENT_PARAMETER_OPERATION_STATE = new WindowsDevicePropertyKeys(0x15AB1953, 0xF817, 0x4FEF, 0xA9, 0x21, 0x56, 0x76, 0xE8, 0x38, 0xF6, 0xE0, 4);
        public static WindowsDevicePropertyKeys WPD_EVENT_PARAMETER_OPERATION_PROGRESS = new WindowsDevicePropertyKeys(0x15AB1953, 0xF817, 0x4FEF, 0xA9, 0x21, 0x56, 0x76, 0xE8, 0x38, 0xF6, 0xE0, 5);
        public static WindowsDevicePropertyKeys WPD_EVENT_PARAMETER_OBJECT_PARENT_PERSISTENT_UNIQUE_ID = new WindowsDevicePropertyKeys(0x15AB1953, 0xF817, 0x4FEF, 0xA9, 0x21, 0x56, 0x76, 0xE8, 0x38, 0xF6, 0xE0, 6);
        public static WindowsDevicePropertyKeys WPD_EVENT_PARAMETER_OBJECT_CREATION_COOKIE = new WindowsDevicePropertyKeys(0x15AB1953, 0xF817, 0x4FEF, 0xA9, 0x21, 0x56, 0x76, 0xE8, 0x38, 0xF6, 0xE0, 7);
        public static WindowsDevicePropertyKeys WPD_EVENT_PARAMETER_CHILD_HIERARCHY_CHANGED = new WindowsDevicePropertyKeys(0x15AB1953, 0xF817, 0x4FEF, 0xA9, 0x21, 0x56, 0x76, 0xE8, 0x38, 0xF6, 0xE0, 8);
        public static WindowsDevicePropertyKeys WPD_EVENT_PARAMETER_SERVICE_METHOD_CONTEXT = new WindowsDevicePropertyKeys(0x52807B8A, 0x4914, 0x4323, 0x9B, 0x9A, 0x74, 0xF6, 0x54, 0xB2, 0xB8, 0x46, 2);
        public static WindowsDevicePropertyKeys WPD_EVENT_OPTION_IS_BROADCAST_EVENT = new WindowsDevicePropertyKeys(0xB3D8DAD7, 0xA361, 0x4B83, 0x8A, 0x48, 0x5B, 0x02, 0xCE, 0x10, 0x71, 0x3B, 2);
        public static WindowsDevicePropertyKeys WPD_EVENT_OPTION_IS_AUTOPLAY_EVENT = new WindowsDevicePropertyKeys(0xB3D8DAD7, 0xA361, 0x4B83, 0x8A, 0x48, 0x5B, 0x02, 0xCE, 0x10, 0x71, 0x3B, 3);
        public static WindowsDevicePropertyKeys WPD_EVENT_ATTRIBUTE_NAME = new WindowsDevicePropertyKeys(0x10C96578, 0x2E81, 0x4111, 0xAD, 0xDE, 0xE0, 0x8C, 0xA6, 0x13, 0x8F, 0x6D, 2);
        public static WindowsDevicePropertyKeys WPD_EVENT_ATTRIBUTE_PARAMETERS = new WindowsDevicePropertyKeys(0x10C96578, 0x2E81, 0x4111, 0xAD, 0xDE, 0xE0, 0x8C, 0xA6, 0x13, 0x8F, 0x6D, 3);
        public static WindowsDevicePropertyKeys WPD_EVENT_ATTRIBUTE_OPTIONS = new WindowsDevicePropertyKeys(0x10C96578, 0x2E81, 0x4111, 0xAD, 0xDE, 0xE0, 0x8C, 0xA6, 0x13, 0x8F, 0x6D, 4);
        public static WindowsDevicePropertyKeys WPD_API_OPTION_USE_CLEAR_DATA_STREAM = new WindowsDevicePropertyKeys(0x10E54A3E, 0x052D, 0x4777, 0xA1, 0x3C, 0xDE, 0x76, 0x14, 0xBE, 0x2B, 0xC4, 2);
        public static WindowsDevicePropertyKeys WPD_API_OPTION_IOCTL_ACCESS = new WindowsDevicePropertyKeys(0x10E54A3E, 0x052D, 0x4777, 0xA1, 0x3C, 0xDE, 0x76, 0x14, 0xBE, 0x2B, 0xC4, 3);
        public static WindowsDevicePropertyKeys WPD_FORMAT_ATTRIBUTE_NAME = new WindowsDevicePropertyKeys(0xA0A02000, 0xBCAF, 0x4BE8, 0xB3, 0xF5, 0x23, 0x3F, 0x23, 0x1C, 0xF5, 0x8F, 2);
        public static WindowsDevicePropertyKeys WPD_FORMAT_ATTRIBUTE_MIMETYPE = new WindowsDevicePropertyKeys(0xA0A02000, 0xBCAF, 0x4BE8, 0xB3, 0xF5, 0x23, 0x3F, 0x23, 0x1C, 0xF5, 0x8F, 3);
        public static WindowsDevicePropertyKeys WPD_METHOD_ATTRIBUTE_NAME = new WindowsDevicePropertyKeys(0xF17A5071, 0xF039, 0x44AF, 0x8E, 0xFE, 0x43, 0x2C, 0xF3, 0x2E, 0x43, 0x2A, 2);
        public static WindowsDevicePropertyKeys WPD_METHOD_ATTRIBUTE_ASSOCIATED_FORMAT = new WindowsDevicePropertyKeys(0xF17A5071, 0xF039, 0x44AF, 0x8E, 0xFE, 0x43, 0x2C, 0xF3, 0x2E, 0x43, 0x2A, 3);
        public static WindowsDevicePropertyKeys WPD_METHOD_ATTRIBUTE_ACCESS = new WindowsDevicePropertyKeys(0xF17A5071, 0xF039, 0x44AF, 0x8E, 0xFE, 0x43, 0x2C, 0xF3, 0x2E, 0x43, 0x2A, 4);
        public static WindowsDevicePropertyKeys WPD_METHOD_ATTRIBUTE_PARAMETERS = new WindowsDevicePropertyKeys(0xF17A5071, 0xF039, 0x44AF, 0x8E, 0xFE, 0x43, 0x2C, 0xF3, 0x2E, 0x43, 0x2A, 5);
        public static WindowsDevicePropertyKeys WPD_PARAMETER_ATTRIBUTE_ORDER = new WindowsDevicePropertyKeys(0xE6864DD7, 0xF325, 0x45EA, 0xA1, 0xD5, 0x97, 0xCF, 0x73, 0xB6, 0xCA, 0x58, 2);
        public static WindowsDevicePropertyKeys WPD_PARAMETER_ATTRIBUTE_USAGE = new WindowsDevicePropertyKeys(0xE6864DD7, 0xF325, 0x45EA, 0xA1, 0xD5, 0x97, 0xCF, 0x73, 0xB6, 0xCA, 0x58, 3);
        public static WindowsDevicePropertyKeys WPD_PARAMETER_ATTRIBUTE_FORM = new WindowsDevicePropertyKeys(0xE6864DD7, 0xF325, 0x45EA, 0xA1, 0xD5, 0x97, 0xCF, 0x73, 0xB6, 0xCA, 0x58, 4);
        public static WindowsDevicePropertyKeys WPD_PARAMETER_ATTRIBUTE_DEFAULT_VALUE = new WindowsDevicePropertyKeys(0xE6864DD7, 0xF325, 0x45EA, 0xA1, 0xD5, 0x97, 0xCF, 0x73, 0xB6, 0xCA, 0x58, 5);
        public static WindowsDevicePropertyKeys WPD_PARAMETER_ATTRIBUTE_RANGE_MIN = new WindowsDevicePropertyKeys(0xE6864DD7, 0xF325, 0x45EA, 0xA1, 0xD5, 0x97, 0xCF, 0x73, 0xB6, 0xCA, 0x58, 6);
        public static WindowsDevicePropertyKeys WPD_PARAMETER_ATTRIBUTE_RANGE_MAX = new WindowsDevicePropertyKeys(0xE6864DD7, 0xF325, 0x45EA, 0xA1, 0xD5, 0x97, 0xCF, 0x73, 0xB6, 0xCA, 0x58, 7);
        public static WindowsDevicePropertyKeys WPD_PARAMETER_ATTRIBUTE_RANGE_STEP = new WindowsDevicePropertyKeys(0xE6864DD7, 0xF325, 0x45EA, 0xA1, 0xD5, 0x97, 0xCF, 0x73, 0xB6, 0xCA, 0x58, 8);
        public static WindowsDevicePropertyKeys WPD_PARAMETER_ATTRIBUTE_ENUMERATION_ELEMENTS = new WindowsDevicePropertyKeys(0xE6864DD7, 0xF325, 0x45EA, 0xA1, 0xD5, 0x97, 0xCF, 0x73, 0xB6, 0xCA, 0x58, 9);
        public static WindowsDevicePropertyKeys WPD_PARAMETER_ATTRIBUTE_REGULAR_EXPRESSION = new WindowsDevicePropertyKeys(0xE6864DD7, 0xF325, 0x45EA, 0xA1, 0xD5, 0x97, 0xCF, 0x73, 0xB6, 0xCA, 0x58, 10);
        public static WindowsDevicePropertyKeys WPD_PARAMETER_ATTRIBUTE_MAX_SIZE = new WindowsDevicePropertyKeys(0xE6864DD7, 0xF325, 0x45EA, 0xA1, 0xD5, 0x97, 0xCF, 0x73, 0xB6, 0xCA, 0x58, 11);
        public static WindowsDevicePropertyKeys WPD_PARAMETER_ATTRIBUTE_VARTYPE = new WindowsDevicePropertyKeys(0xE6864DD7, 0xF325, 0x45EA, 0xA1, 0xD5, 0x97, 0xCF, 0x73, 0xB6, 0xCA, 0x58, 12);
        public static WindowsDevicePropertyKeys WPD_PARAMETER_ATTRIBUTE_NAME = new WindowsDevicePropertyKeys(0xE6864DD7, 0xF325, 0x45EA, 0xA1, 0xD5, 0x97, 0xCF, 0x73, 0xB6, 0xCA, 0x58, 13);
        public static WindowsDevicePropertyKeys WPD_COMMAND_COMMON_RESET_DEVICE = new WindowsDevicePropertyKeys(0xF0422A9C, 0x5DC8, 0x4440, 0xB5, 0xBD, 0x5D, 0xF2, 0x88, 0x35, 0x65, 0x8A, 2);
        public static WindowsDevicePropertyKeys WPD_COMMAND_COMMON_GET_OBJECT_IDS_FROM_PERSISTENT_UNIQUE_IDS = new WindowsDevicePropertyKeys(0xF0422A9C, 0x5DC8, 0x4440, 0xB5, 0xBD, 0x5D, 0xF2, 0x88, 0x35, 0x65, 0x8A, 3);
        public static WindowsDevicePropertyKeys WPD_COMMAND_COMMON_SAVE_CLIENT_INFORMATION = new WindowsDevicePropertyKeys(0xF0422A9C, 0x5DC8, 0x4440, 0xB5, 0xBD, 0x5D, 0xF2, 0x88, 0x35, 0x65, 0x8A, 4);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_COMMON_COMMAND_CATEGORY = new WindowsDevicePropertyKeys(0xF0422A9C, 0x5DC8, 0x4440, 0xB5, 0xBD, 0x5D, 0xF2, 0x88, 0x35, 0x65, 0x8A, 1001);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_COMMON_COMMAND_ID = new WindowsDevicePropertyKeys(0xF0422A9C, 0x5DC8, 0x4440, 0xB5, 0xBD, 0x5D, 0xF2, 0x88, 0x35, 0x65, 0x8A, 1002);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_COMMON_HRESULT = new WindowsDevicePropertyKeys(0xF0422A9C, 0x5DC8, 0x4440, 0xB5, 0xBD, 0x5D, 0xF2, 0x88, 0x35, 0x65, 0x8A, 1003);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_COMMON_DRIVER_ERROR_CODE = new WindowsDevicePropertyKeys(0xF0422A9C, 0x5DC8, 0x4440, 0xB5, 0xBD, 0x5D, 0xF2, 0x88, 0x35, 0x65, 0x8A, 1004);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_COMMON_COMMAND_TARGET = new WindowsDevicePropertyKeys(0xF0422A9C, 0x5DC8, 0x4440, 0xB5, 0xBD, 0x5D, 0xF2, 0x88, 0x35, 0x65, 0x8A, 1006);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_COMMON_PERSISTENT_UNIQUE_IDS = new WindowsDevicePropertyKeys(0xF0422A9C, 0x5DC8, 0x4440, 0xB5, 0xBD, 0x5D, 0xF2, 0x88, 0x35, 0x65, 0x8A, 1007);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_COMMON_OBJECT_IDS = new WindowsDevicePropertyKeys(0xF0422A9C, 0x5DC8, 0x4440, 0xB5, 0xBD, 0x5D, 0xF2, 0x88, 0x35, 0x65, 0x8A, 1008);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_COMMON_CLIENT_INFORMATION = new WindowsDevicePropertyKeys(0xF0422A9C, 0x5DC8, 0x4440, 0xB5, 0xBD, 0x5D, 0xF2, 0x88, 0x35, 0x65, 0x8A, 1009);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_COMMON_CLIENT_INFORMATION_CONTEXT = new WindowsDevicePropertyKeys(0xF0422A9C, 0x5DC8, 0x4440, 0xB5, 0xBD, 0x5D, 0xF2, 0x88, 0x35, 0x65, 0x8A, 1010);
        public static WindowsDevicePropertyKeys WPD_OPTION_VALID_OBJECT_IDS = new WindowsDevicePropertyKeys(0xF0422A9C, 0x5DC8, 0x4440, 0xB5, 0xBD, 0x5D, 0xF2, 0x88, 0x35, 0x65, 0x8A, 5001);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_ENUMERATION_START_FIND = new WindowsDevicePropertyKeys(0xB7474E91, 0xE7F8, 0x4AD9, 0xB4, 0x00, 0xAD, 0x1A, 0x4B, 0x58, 0xEE, 0xEC, 2);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_ENUMERATION_FIND_NEXT = new WindowsDevicePropertyKeys(0xB7474E91, 0xE7F8, 0x4AD9, 0xB4, 0x00, 0xAD, 0x1A, 0x4B, 0x58, 0xEE, 0xEC, 3);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_ENUMERATION_END_FIND = new WindowsDevicePropertyKeys(0xB7474E91, 0xE7F8, 0x4AD9, 0xB4, 0x00, 0xAD, 0x1A, 0x4B, 0x58, 0xEE, 0xEC, 4);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_ENUMERATION_PARENT_ID = new WindowsDevicePropertyKeys(0xB7474E91, 0xE7F8, 0x4AD9, 0xB4, 0x00, 0xAD, 0x1A, 0x4B, 0x58, 0xEE, 0xEC, 1001);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_ENUMERATION_FILTER = new WindowsDevicePropertyKeys(0xB7474E91, 0xE7F8, 0x4AD9, 0xB4, 0x00, 0xAD, 0x1A, 0x4B, 0x58, 0xEE, 0xEC, 1002);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_ENUMERATION_OBJECT_IDS = new WindowsDevicePropertyKeys(0xB7474E91, 0xE7F8, 0x4AD9, 0xB4, 0x00, 0xAD, 0x1A, 0x4B, 0x58, 0xEE, 0xEC, 1003);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_ENUMERATION_CONTEXT = new WindowsDevicePropertyKeys(0xB7474E91, 0xE7F8, 0x4AD9, 0xB4, 0x00, 0xAD, 0x1A, 0x4B, 0x58, 0xEE, 0xEC, 1004);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_ENUMERATION_NUM_OBJECTS_REQUESTED = new WindowsDevicePropertyKeys(0xB7474E91, 0xE7F8, 0x4AD9, 0xB4, 0x00, 0xAD, 0x1A, 0x4B, 0x58, 0xEE, 0xEC, 1005);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_PROPERTIES_GET_SUPPORTED = new WindowsDevicePropertyKeys(0x9E5582E4, 0x0814, 0x44E6, 0x98, 0x1A, 0xB2, 0x99, 0x8D, 0x58, 0x38, 0x04, 2);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_PROPERTIES_GET_ATTRIBUTES = new WindowsDevicePropertyKeys(0x9E5582E4, 0x0814, 0x44E6, 0x98, 0x1A, 0xB2, 0x99, 0x8D, 0x58, 0x38, 0x04, 3);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_PROPERTIES_GET = new WindowsDevicePropertyKeys(0x9E5582E4, 0x0814, 0x44E6, 0x98, 0x1A, 0xB2, 0x99, 0x8D, 0x58, 0x38, 0x04, 4);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_PROPERTIES_SET = new WindowsDevicePropertyKeys(0x9E5582E4, 0x0814, 0x44E6, 0x98, 0x1A, 0xB2, 0x99, 0x8D, 0x58, 0x38, 0x04, 5);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_PROPERTIES_GET_ALL = new WindowsDevicePropertyKeys(0x9E5582E4, 0x0814, 0x44E6, 0x98, 0x1A, 0xB2, 0x99, 0x8D, 0x58, 0x38, 0x04, 6);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_PROPERTIES_DELETE = new WindowsDevicePropertyKeys(0x9E5582E4, 0x0814, 0x44E6, 0x98, 0x1A, 0xB2, 0x99, 0x8D, 0x58, 0x38, 0x04, 7);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_PROPERTIES_OBJECT_ID = new WindowsDevicePropertyKeys(0x9E5582E4, 0x0814, 0x44E6, 0x98, 0x1A, 0xB2, 0x99, 0x8D, 0x58, 0x38, 0x04, 1001);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_PROPERTIES_PROPERTY_KEYS = new WindowsDevicePropertyKeys(0x9E5582E4, 0x0814, 0x44E6, 0x98, 0x1A, 0xB2, 0x99, 0x8D, 0x58, 0x38, 0x04, 1002);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_PROPERTIES_PROPERTY_ATTRIBUTES = new WindowsDevicePropertyKeys(0x9E5582E4, 0x0814, 0x44E6, 0x98, 0x1A, 0xB2, 0x99, 0x8D, 0x58, 0x38, 0x04, 1003);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_PROPERTIES_PROPERTY_VALUES = new WindowsDevicePropertyKeys(0x9E5582E4, 0x0814, 0x44E6, 0x98, 0x1A, 0xB2, 0x99, 0x8D, 0x58, 0x38, 0x04, 1004);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_PROPERTIES_PROPERTY_WRITE_RESULTS = new WindowsDevicePropertyKeys(0x9E5582E4, 0x0814, 0x44E6, 0x98, 0x1A, 0xB2, 0x99, 0x8D, 0x58, 0x38, 0x04, 1005);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_PROPERTIES_PROPERTY_DELETE_RESULTS = new WindowsDevicePropertyKeys(0x9E5582E4, 0x0814, 0x44E6, 0x98, 0x1A, 0xB2, 0x99, 0x8D, 0x58, 0x38, 0x04, 1006);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_PROPERTIES_BULK_GET_VALUES_BY_OBJECT_LIST_START = new WindowsDevicePropertyKeys(0x11C824DD, 0x04CD, 0x4E4E, 0x8C, 0x7B, 0xF6, 0xEF, 0xB7, 0x94, 0xD8, 0x4E, 2);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_PROPERTIES_BULK_GET_VALUES_BY_OBJECT_LIST_NEXT = new WindowsDevicePropertyKeys(0x11C824DD, 0x04CD, 0x4E4E, 0x8C, 0x7B, 0xF6, 0xEF, 0xB7, 0x94, 0xD8, 0x4E, 3);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_PROPERTIES_BULK_GET_VALUES_BY_OBJECT_LIST_END = new WindowsDevicePropertyKeys(0x11C824DD, 0x04CD, 0x4E4E, 0x8C, 0x7B, 0xF6, 0xEF, 0xB7, 0x94, 0xD8, 0x4E, 4);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_PROPERTIES_BULK_GET_VALUES_BY_OBJECT_FORMAT_START = new WindowsDevicePropertyKeys(0x11C824DD, 0x04CD, 0x4E4E, 0x8C, 0x7B, 0xF6, 0xEF, 0xB7, 0x94, 0xD8, 0x4E, 5);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_PROPERTIES_BULK_GET_VALUES_BY_OBJECT_FORMAT_NEXT = new WindowsDevicePropertyKeys(0x11C824DD, 0x04CD, 0x4E4E, 0x8C, 0x7B, 0xF6, 0xEF, 0xB7, 0x94, 0xD8, 0x4E, 6);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_PROPERTIES_BULK_GET_VALUES_BY_OBJECT_FORMAT_END = new WindowsDevicePropertyKeys(0x11C824DD, 0x04CD, 0x4E4E, 0x8C, 0x7B, 0xF6, 0xEF, 0xB7, 0x94, 0xD8, 0x4E, 7);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_PROPERTIES_BULK_SET_VALUES_BY_OBJECT_LIST_START = new WindowsDevicePropertyKeys(0x11C824DD, 0x04CD, 0x4E4E, 0x8C, 0x7B, 0xF6, 0xEF, 0xB7, 0x94, 0xD8, 0x4E, 8);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_PROPERTIES_BULK_SET_VALUES_BY_OBJECT_LIST_NEXT = new WindowsDevicePropertyKeys(0x11C824DD, 0x04CD, 0x4E4E, 0x8C, 0x7B, 0xF6, 0xEF, 0xB7, 0x94, 0xD8, 0x4E, 9);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_PROPERTIES_BULK_SET_VALUES_BY_OBJECT_LIST_END = new WindowsDevicePropertyKeys(0x11C824DD, 0x04CD, 0x4E4E, 0x8C, 0x7B, 0xF6, 0xEF, 0xB7, 0x94, 0xD8, 0x4E, 10);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_PROPERTIES_BULK_OBJECT_IDS = new WindowsDevicePropertyKeys(0x11C824DD, 0x04CD, 0x4E4E, 0x8C, 0x7B, 0xF6, 0xEF, 0xB7, 0x94, 0xD8, 0x4E, 1001);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_PROPERTIES_BULK_CONTEXT = new WindowsDevicePropertyKeys(0x11C824DD, 0x04CD, 0x4E4E, 0x8C, 0x7B, 0xF6, 0xEF, 0xB7, 0x94, 0xD8, 0x4E, 1002);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_PROPERTIES_BULK_VALUES = new WindowsDevicePropertyKeys(0x11C824DD, 0x04CD, 0x4E4E, 0x8C, 0x7B, 0xF6, 0xEF, 0xB7, 0x94, 0xD8, 0x4E, 1003);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_PROPERTIES_BULK_PROPERTY_KEYS = new WindowsDevicePropertyKeys(0x11C824DD, 0x04CD, 0x4E4E, 0x8C, 0x7B, 0xF6, 0xEF, 0xB7, 0x94, 0xD8, 0x4E, 1004);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_PROPERTIES_BULK_DEPTH = new WindowsDevicePropertyKeys(0x11C824DD, 0x04CD, 0x4E4E, 0x8C, 0x7B, 0xF6, 0xEF, 0xB7, 0x94, 0xD8, 0x4E, 1005);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_PROPERTIES_BULK_PARENT_OBJECT_ID = new WindowsDevicePropertyKeys(0x11C824DD, 0x04CD, 0x4E4E, 0x8C, 0x7B, 0xF6, 0xEF, 0xB7, 0x94, 0xD8, 0x4E, 1006);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_PROPERTIES_BULK_OBJECT_FORMAT = new WindowsDevicePropertyKeys(0x11C824DD, 0x04CD, 0x4E4E, 0x8C, 0x7B, 0xF6, 0xEF, 0xB7, 0x94, 0xD8, 0x4E, 1007);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_PROPERTIES_BULK_WRITE_RESULTS = new WindowsDevicePropertyKeys(0x11C824DD, 0x04CD, 0x4E4E, 0x8C, 0x7B, 0xF6, 0xEF, 0xB7, 0x94, 0xD8, 0x4E, 1008);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_RESOURCES_GET_SUPPORTED = new WindowsDevicePropertyKeys(0xB3A2B22D, 0xA595, 0x4108, 0xBE, 0x0A, 0xFC, 0x3C, 0x96, 0x5F, 0x3D, 0x4A, 2);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_RESOURCES_GET_ATTRIBUTES = new WindowsDevicePropertyKeys(0xB3A2B22D, 0xA595, 0x4108, 0xBE, 0x0A, 0xFC, 0x3C, 0x96, 0x5F, 0x3D, 0x4A, 3);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_RESOURCES_OPEN = new WindowsDevicePropertyKeys(0xB3A2B22D, 0xA595, 0x4108, 0xBE, 0x0A, 0xFC, 0x3C, 0x96, 0x5F, 0x3D, 0x4A, 4);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_RESOURCES_READ = new WindowsDevicePropertyKeys(0xB3A2B22D, 0xA595, 0x4108, 0xBE, 0x0A, 0xFC, 0x3C, 0x96, 0x5F, 0x3D, 0x4A, 5);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_RESOURCES_WRITE = new WindowsDevicePropertyKeys(0xB3A2B22D, 0xA595, 0x4108, 0xBE, 0x0A, 0xFC, 0x3C, 0x96, 0x5F, 0x3D, 0x4A, 6);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_RESOURCES_CLOSE = new WindowsDevicePropertyKeys(0xB3A2B22D, 0xA595, 0x4108, 0xBE, 0x0A, 0xFC, 0x3C, 0x96, 0x5F, 0x3D, 0x4A, 7);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_RESOURCES_DELETE = new WindowsDevicePropertyKeys(0xB3A2B22D, 0xA595, 0x4108, 0xBE, 0x0A, 0xFC, 0x3C, 0x96, 0x5F, 0x3D, 0x4A, 8);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_RESOURCES_CREATE_RESOURCE = new WindowsDevicePropertyKeys(0xB3A2B22D, 0xA595, 0x4108, 0xBE, 0x0A, 0xFC, 0x3C, 0x96, 0x5F, 0x3D, 0x4A, 9);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_RESOURCES_REVERT = new WindowsDevicePropertyKeys(0xB3A2B22D, 0xA595, 0x4108, 0xBE, 0x0A, 0xFC, 0x3C, 0x96, 0x5F, 0x3D, 0x4A, 10);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_RESOURCES_SEEK = new WindowsDevicePropertyKeys(0xB3A2B22D, 0xA595, 0x4108, 0xBE, 0x0A, 0xFC, 0x3C, 0x96, 0x5F, 0x3D, 0x4A, 11);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_RESOURCES_OBJECT_ID = new WindowsDevicePropertyKeys(0xB3A2B22D, 0xA595, 0x4108, 0xBE, 0x0A, 0xFC, 0x3C, 0x96, 0x5F, 0x3D, 0x4A, 1001);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_RESOURCES_ACCESS_MODE = new WindowsDevicePropertyKeys(0xB3A2B22D, 0xA595, 0x4108, 0xBE, 0x0A, 0xFC, 0x3C, 0x96, 0x5F, 0x3D, 0x4A, 1002);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_RESOURCES_RESOURCE_KEYS = new WindowsDevicePropertyKeys(0xB3A2B22D, 0xA595, 0x4108, 0xBE, 0x0A, 0xFC, 0x3C, 0x96, 0x5F, 0x3D, 0x4A, 1003);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_RESOURCES_RESOURCE_ATTRIBUTES = new WindowsDevicePropertyKeys(0xB3A2B22D, 0xA595, 0x4108, 0xBE, 0x0A, 0xFC, 0x3C, 0x96, 0x5F, 0x3D, 0x4A, 1004);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_RESOURCES_CONTEXT = new WindowsDevicePropertyKeys(0xB3A2B22D, 0xA595, 0x4108, 0xBE, 0x0A, 0xFC, 0x3C, 0x96, 0x5F, 0x3D, 0x4A, 1005);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_RESOURCES_NUM_BYTES_TO_READ = new WindowsDevicePropertyKeys(0xB3A2B22D, 0xA595, 0x4108, 0xBE, 0x0A, 0xFC, 0x3C, 0x96, 0x5F, 0x3D, 0x4A, 1006);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_RESOURCES_NUM_BYTES_READ = new WindowsDevicePropertyKeys(0xB3A2B22D, 0xA595, 0x4108, 0xBE, 0x0A, 0xFC, 0x3C, 0x96, 0x5F, 0x3D, 0x4A, 1007);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_RESOURCES_NUM_BYTES_TO_WRITE = new WindowsDevicePropertyKeys(0xB3A2B22D, 0xA595, 0x4108, 0xBE, 0x0A, 0xFC, 0x3C, 0x96, 0x5F, 0x3D, 0x4A, 1008);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_RESOURCES_NUM_BYTES_WRITTEN = new WindowsDevicePropertyKeys(0xB3A2B22D, 0xA595, 0x4108, 0xBE, 0x0A, 0xFC, 0x3C, 0x96, 0x5F, 0x3D, 0x4A, 1009);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_RESOURCES_DATA = new WindowsDevicePropertyKeys(0xB3A2B22D, 0xA595, 0x4108, 0xBE, 0x0A, 0xFC, 0x3C, 0x96, 0x5F, 0x3D, 0x4A, 1010);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_RESOURCES_OPTIMAL_TRANSFER_BUFFER_SIZE = new WindowsDevicePropertyKeys(0xB3A2B22D, 0xA595, 0x4108, 0xBE, 0x0A, 0xFC, 0x3C, 0x96, 0x5F, 0x3D, 0x4A, 1011);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_RESOURCES_SEEK_OFFSET = new WindowsDevicePropertyKeys(0xB3A2B22D, 0xA595, 0x4108, 0xBE, 0x0A, 0xFC, 0x3C, 0x96, 0x5F, 0x3D, 0x4A, 1012);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_RESOURCES_SEEK_ORIGIN_FLAG = new WindowsDevicePropertyKeys(0xB3A2B22D, 0xA595, 0x4108, 0xBE, 0x0A, 0xFC, 0x3C, 0x96, 0x5F, 0x3D, 0x4A, 1013);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_RESOURCES_POSITION_FROM_START = new WindowsDevicePropertyKeys(0xB3A2B22D, 0xA595, 0x4108, 0xBE, 0x0A, 0xFC, 0x3C, 0x96, 0x5F, 0x3D, 0x4A, 1014);
        public static WindowsDevicePropertyKeys WPD_OPTION_OBJECT_RESOURCES_SEEK_ON_READ_SUPPORTED = new WindowsDevicePropertyKeys(0xB3A2B22D, 0xA595, 0x4108, 0xBE, 0x0A, 0xFC, 0x3C, 0x96, 0x5F, 0x3D, 0x4A, 5001);
        public static WindowsDevicePropertyKeys WPD_OPTION_OBJECT_RESOURCES_SEEK_ON_WRITE_SUPPORTED = new WindowsDevicePropertyKeys(0xB3A2B22D, 0xA595, 0x4108, 0xBE, 0x0A, 0xFC, 0x3C, 0x96, 0x5F, 0x3D, 0x4A, 5002);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_MANAGEMENT_CREATE_OBJECT_WITH_PROPERTIES_ONLY = new WindowsDevicePropertyKeys(0xEF1E43DD, 0xA9ED, 0x4341, 0x8B, 0xCC, 0x18, 0x61, 0x92, 0xAE, 0xA0, 0x89, 2);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_MANAGEMENT_CREATE_OBJECT_WITH_PROPERTIES_AND_DATA = new WindowsDevicePropertyKeys(0xEF1E43DD, 0xA9ED, 0x4341, 0x8B, 0xCC, 0x18, 0x61, 0x92, 0xAE, 0xA0, 0x89, 3);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_MANAGEMENT_WRITE_OBJECT_DATA = new WindowsDevicePropertyKeys(0xEF1E43DD, 0xA9ED, 0x4341, 0x8B, 0xCC, 0x18, 0x61, 0x92, 0xAE, 0xA0, 0x89, 4);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_MANAGEMENT_COMMIT_OBJECT = new WindowsDevicePropertyKeys(0xEF1E43DD, 0xA9ED, 0x4341, 0x8B, 0xCC, 0x18, 0x61, 0x92, 0xAE, 0xA0, 0x89, 5);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_MANAGEMENT_REVERT_OBJECT = new WindowsDevicePropertyKeys(0xEF1E43DD, 0xA9ED, 0x4341, 0x8B, 0xCC, 0x18, 0x61, 0x92, 0xAE, 0xA0, 0x89, 6);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_MANAGEMENT_DELETE_OBJECTS = new WindowsDevicePropertyKeys(0xEF1E43DD, 0xA9ED, 0x4341, 0x8B, 0xCC, 0x18, 0x61, 0x92, 0xAE, 0xA0, 0x89, 7);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_MANAGEMENT_MOVE_OBJECTS = new WindowsDevicePropertyKeys(0xEF1E43DD, 0xA9ED, 0x4341, 0x8B, 0xCC, 0x18, 0x61, 0x92, 0xAE, 0xA0, 0x89, 8);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_MANAGEMENT_COPY_OBJECTS = new WindowsDevicePropertyKeys(0xEF1E43DD, 0xA9ED, 0x4341, 0x8B, 0xCC, 0x18, 0x61, 0x92, 0xAE, 0xA0, 0x89, 9);
        public static WindowsDevicePropertyKeys WPD_COMMAND_OBJECT_MANAGEMENT_UPDATE_OBJECT_WITH_PROPERTIES_AND_DATA = new WindowsDevicePropertyKeys(0xEF1E43DD, 0xA9ED, 0x4341, 0x8B, 0xCC, 0x18, 0x61, 0x92, 0xAE, 0xA0, 0x89, 10);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_MANAGEMENT_CREATION_PROPERTIES = new WindowsDevicePropertyKeys(0xEF1E43DD, 0xA9ED, 0x4341, 0x8B, 0xCC, 0x18, 0x61, 0x92, 0xAE, 0xA0, 0x89, 1001);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_MANAGEMENT_CONTEXT = new WindowsDevicePropertyKeys(0xEF1E43DD, 0xA9ED, 0x4341, 0x8B, 0xCC, 0x18, 0x61, 0x92, 0xAE, 0xA0, 0x89, 1002);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_MANAGEMENT_NUM_BYTES_TO_WRITE = new WindowsDevicePropertyKeys(0xEF1E43DD, 0xA9ED, 0x4341, 0x8B, 0xCC, 0x18, 0x61, 0x92, 0xAE, 0xA0, 0x89, 1003);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_MANAGEMENT_NUM_BYTES_WRITTEN = new WindowsDevicePropertyKeys(0xEF1E43DD, 0xA9ED, 0x4341, 0x8B, 0xCC, 0x18, 0x61, 0x92, 0xAE, 0xA0, 0x89, 1004);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_MANAGEMENT_DATA = new WindowsDevicePropertyKeys(0xEF1E43DD, 0xA9ED, 0x4341, 0x8B, 0xCC, 0x18, 0x61, 0x92, 0xAE, 0xA0, 0x89, 1005);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_MANAGEMENT_OBJECT_ID = new WindowsDevicePropertyKeys(0xEF1E43DD, 0xA9ED, 0x4341, 0x8B, 0xCC, 0x18, 0x61, 0x92, 0xAE, 0xA0, 0x89, 1006);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_MANAGEMENT_DELETE_OPTIONS = new WindowsDevicePropertyKeys(0xEF1E43DD, 0xA9ED, 0x4341, 0x8B, 0xCC, 0x18, 0x61, 0x92, 0xAE, 0xA0, 0x89, 1007);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_MANAGEMENT_OPTIMAL_TRANSFER_BUFFER_SIZE = new WindowsDevicePropertyKeys(0xEF1E43DD, 0xA9ED, 0x4341, 0x8B, 0xCC, 0x18, 0x61, 0x92, 0xAE, 0xA0, 0x89, 1008);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_MANAGEMENT_OBJECT_IDS = new WindowsDevicePropertyKeys(0xEF1E43DD, 0xA9ED, 0x4341, 0x8B, 0xCC, 0x18, 0x61, 0x92, 0xAE, 0xA0, 0x89, 1009);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_MANAGEMENT_DELETE_RESULTS = new WindowsDevicePropertyKeys(0xEF1E43DD, 0xA9ED, 0x4341, 0x8B, 0xCC, 0x18, 0x61, 0x92, 0xAE, 0xA0, 0x89, 1010);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_MANAGEMENT_DESTINATION_FOLDER_OBJECT_ID = new WindowsDevicePropertyKeys(0xEF1E43DD, 0xA9ED, 0x4341, 0x8B, 0xCC, 0x18, 0x61, 0x92, 0xAE, 0xA0, 0x89, 1011);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_MANAGEMENT_MOVE_RESULTS = new WindowsDevicePropertyKeys(0xEF1E43DD, 0xA9ED, 0x4341, 0x8B, 0xCC, 0x18, 0x61, 0x92, 0xAE, 0xA0, 0x89, 1012);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_MANAGEMENT_COPY_RESULTS = new WindowsDevicePropertyKeys(0xEF1E43DD, 0xA9ED, 0x4341, 0x8B, 0xCC, 0x18, 0x61, 0x92, 0xAE, 0xA0, 0x89, 1013);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_MANAGEMENT_UPDATE_PROPERTIES = new WindowsDevicePropertyKeys(0xEF1E43DD, 0xA9ED, 0x4341, 0x8B, 0xCC, 0x18, 0x61, 0x92, 0xAE, 0xA0, 0x89, 1014);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_MANAGEMENT_PROPERTY_KEYS = new WindowsDevicePropertyKeys(0xEF1E43DD, 0xA9ED, 0x4341, 0x8B, 0xCC, 0x18, 0x61, 0x92, 0xAE, 0xA0, 0x89, 1015);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_OBJECT_MANAGEMENT_OBJECT_FORMAT = new WindowsDevicePropertyKeys(0xEF1E43DD, 0xA9ED, 0x4341, 0x8B, 0xCC, 0x18, 0x61, 0x92, 0xAE, 0xA0, 0x89, 1016);
        public static WindowsDevicePropertyKeys WPD_OPTION_OBJECT_MANAGEMENT_RECURSIVE_DELETE_SUPPORTED = new WindowsDevicePropertyKeys(0xEF1E43DD, 0xA9ED, 0x4341, 0x8B, 0xCC, 0x18, 0x61, 0x92, 0xAE, 0xA0, 0x89, 5001);
        public static WindowsDevicePropertyKeys WPD_COMMAND_CAPABILITIES_GET_SUPPORTED_COMMANDS = new WindowsDevicePropertyKeys(0x0CABEC78, 0x6B74, 0x41C6, 0x92, 0x16, 0x26, 0x39, 0xD1, 0xFC, 0xE3, 0x56, 2);
        public static WindowsDevicePropertyKeys WPD_COMMAND_CAPABILITIES_GET_COMMAND_OPTIONS = new WindowsDevicePropertyKeys(0x0CABEC78, 0x6B74, 0x41C6, 0x92, 0x16, 0x26, 0x39, 0xD1, 0xFC, 0xE3, 0x56, 3);
        public static WindowsDevicePropertyKeys WPD_COMMAND_CAPABILITIES_GET_SUPPORTED_FUNCTIONAL_CATEGORIES = new WindowsDevicePropertyKeys(0x0CABEC78, 0x6B74, 0x41C6, 0x92, 0x16, 0x26, 0x39, 0xD1, 0xFC, 0xE3, 0x56, 4);
        public static WindowsDevicePropertyKeys WPD_COMMAND_CAPABILITIES_GET_FUNCTIONAL_OBJECTS = new WindowsDevicePropertyKeys(0x0CABEC78, 0x6B74, 0x41C6, 0x92, 0x16, 0x26, 0x39, 0xD1, 0xFC, 0xE3, 0x56, 5);
        public static WindowsDevicePropertyKeys WPD_COMMAND_CAPABILITIES_GET_SUPPORTED_CONTENT_TYPES = new WindowsDevicePropertyKeys(0x0CABEC78, 0x6B74, 0x41C6, 0x92, 0x16, 0x26, 0x39, 0xD1, 0xFC, 0xE3, 0x56, 6);
        public static WindowsDevicePropertyKeys WPD_COMMAND_CAPABILITIES_GET_SUPPORTED_FORMATS = new WindowsDevicePropertyKeys(0x0CABEC78, 0x6B74, 0x41C6, 0x92, 0x16, 0x26, 0x39, 0xD1, 0xFC, 0xE3, 0x56, 7);
        public static WindowsDevicePropertyKeys WPD_COMMAND_CAPABILITIES_GET_SUPPORTED_FORMAT_PROPERTIES = new WindowsDevicePropertyKeys(0x0CABEC78, 0x6B74, 0x41C6, 0x92, 0x16, 0x26, 0x39, 0xD1, 0xFC, 0xE3, 0x56, 8);
        public static WindowsDevicePropertyKeys WPD_COMMAND_CAPABILITIES_GET_FIXED_PROPERTY_ATTRIBUTES = new WindowsDevicePropertyKeys(0x0CABEC78, 0x6B74, 0x41C6, 0x92, 0x16, 0x26, 0x39, 0xD1, 0xFC, 0xE3, 0x56, 9);
        public static WindowsDevicePropertyKeys WPD_COMMAND_CAPABILITIES_GET_SUPPORTED_EVENTS = new WindowsDevicePropertyKeys(0x0CABEC78, 0x6B74, 0x41C6, 0x92, 0x16, 0x26, 0x39, 0xD1, 0xFC, 0xE3, 0x56, 10);
        public static WindowsDevicePropertyKeys WPD_COMMAND_CAPABILITIES_GET_EVENT_OPTIONS = new WindowsDevicePropertyKeys(0x0CABEC78, 0x6B74, 0x41C6, 0x92, 0x16, 0x26, 0x39, 0xD1, 0xFC, 0xE3, 0x56, 11);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_CAPABILITIES_SUPPORTED_COMMANDS = new WindowsDevicePropertyKeys(0x0CABEC78, 0x6B74, 0x41C6, 0x92, 0x16, 0x26, 0x39, 0xD1, 0xFC, 0xE3, 0x56, 1001);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_CAPABILITIES_COMMAND = new WindowsDevicePropertyKeys(0x0CABEC78, 0x6B74, 0x41C6, 0x92, 0x16, 0x26, 0x39, 0xD1, 0xFC, 0xE3, 0x56, 1002);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_CAPABILITIES_COMMAND_OPTIONS = new WindowsDevicePropertyKeys(0x0CABEC78, 0x6B74, 0x41C6, 0x92, 0x16, 0x26, 0x39, 0xD1, 0xFC, 0xE3, 0x56, 1003);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_CAPABILITIES_FUNCTIONAL_CATEGORIES = new WindowsDevicePropertyKeys(0x0CABEC78, 0x6B74, 0x41C6, 0x92, 0x16, 0x26, 0x39, 0xD1, 0xFC, 0xE3, 0x56, 1004);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_CAPABILITIES_FUNCTIONAL_CATEGORY = new WindowsDevicePropertyKeys(0x0CABEC78, 0x6B74, 0x41C6, 0x92, 0x16, 0x26, 0x39, 0xD1, 0xFC, 0xE3, 0x56, 1005);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_CAPABILITIES_FUNCTIONAL_OBJECTS = new WindowsDevicePropertyKeys(0x0CABEC78, 0x6B74, 0x41C6, 0x92, 0x16, 0x26, 0x39, 0xD1, 0xFC, 0xE3, 0x56, 1006);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_CAPABILITIES_CONTENT_TYPES = new WindowsDevicePropertyKeys(0x0CABEC78, 0x6B74, 0x41C6, 0x92, 0x16, 0x26, 0x39, 0xD1, 0xFC, 0xE3, 0x56, 1007);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_CAPABILITIES_CONTENT_TYPE = new WindowsDevicePropertyKeys(0x0CABEC78, 0x6B74, 0x41C6, 0x92, 0x16, 0x26, 0x39, 0xD1, 0xFC, 0xE3, 0x56, 1008);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_CAPABILITIES_FORMATS = new WindowsDevicePropertyKeys(0x0CABEC78, 0x6B74, 0x41C6, 0x92, 0x16, 0x26, 0x39, 0xD1, 0xFC, 0xE3, 0x56, 1009);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_CAPABILITIES_FORMAT = new WindowsDevicePropertyKeys(0x0CABEC78, 0x6B74, 0x41C6, 0x92, 0x16, 0x26, 0x39, 0xD1, 0xFC, 0xE3, 0x56, 1010);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_CAPABILITIES_PROPERTY_KEYS = new WindowsDevicePropertyKeys(0x0CABEC78, 0x6B74, 0x41C6, 0x92, 0x16, 0x26, 0x39, 0xD1, 0xFC, 0xE3, 0x56, 1011);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_CAPABILITIES_PROPERTY_ATTRIBUTES = new WindowsDevicePropertyKeys(0x0CABEC78, 0x6B74, 0x41C6, 0x92, 0x16, 0x26, 0x39, 0xD1, 0xFC, 0xE3, 0x56, 1012);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_CAPABILITIES_SUPPORTED_EVENTS = new WindowsDevicePropertyKeys(0x0CABEC78, 0x6B74, 0x41C6, 0x92, 0x16, 0x26, 0x39, 0xD1, 0xFC, 0xE3, 0x56, 1013);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_CAPABILITIES_EVENT = new WindowsDevicePropertyKeys(0x0CABEC78, 0x6B74, 0x41C6, 0x92, 0x16, 0x26, 0x39, 0xD1, 0xFC, 0xE3, 0x56, 1014);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_CAPABILITIES_EVENT_OPTIONS = new WindowsDevicePropertyKeys(0x0CABEC78, 0x6B74, 0x41C6, 0x92, 0x16, 0x26, 0x39, 0xD1, 0xFC, 0xE3, 0x56, 1015);
        public static WindowsDevicePropertyKeys WPD_COMMAND_CLASS_EXTENSION_WRITE_DEVICE_INFORMATION = new WindowsDevicePropertyKeys(0x33FB0D11, 0x64A3, 0x4FAC, 0xB4, 0xC7, 0x3D, 0xFE, 0xAA, 0x99, 0xB0, 0x51, 2);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_CLASS_EXTENSION_DEVICE_INFORMATION_VALUES = new WindowsDevicePropertyKeys(0x33FB0D11, 0x64A3, 0x4FAC, 0xB4, 0xC7, 0x3D, 0xFE, 0xAA, 0x99, 0xB0, 0x51, 1001);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_CLASS_EXTENSION_DEVICE_INFORMATION_WRITE_RESULTS = new WindowsDevicePropertyKeys(0x33FB0D11, 0x64A3, 0x4FAC, 0xB4, 0xC7, 0x3D, 0xFE, 0xAA, 0x99, 0xB0, 0x51, 1002);
        public static WindowsDevicePropertyKeys WPD_COMMAND_CLASS_EXTENSION_REGISTER_SERVICE_INTERFACES = new WindowsDevicePropertyKeys(0x7F0779B5, 0xFA2B, 0x4766, 0x9C, 0xB2, 0xF7, 0x3B, 0xA3, 0x0B, 0x67, 0x58, 2);
        public static WindowsDevicePropertyKeys WPD_COMMAND_CLASS_EXTENSION_UNREGISTER_SERVICE_INTERFACES = new WindowsDevicePropertyKeys(0x7F0779B5, 0xFA2B, 0x4766, 0x9C, 0xB2, 0xF7, 0x3B, 0xA3, 0x0B, 0x67, 0x58, 3);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_CLASS_EXTENSION_SERVICE_OBJECT_ID = new WindowsDevicePropertyKeys(0x7F0779B5, 0xFA2B, 0x4766, 0x9C, 0xB2, 0xF7, 0x3B, 0xA3, 0x0B, 0x67, 0x58, 1001);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_CLASS_EXTENSION_SERVICE_INTERFACES = new WindowsDevicePropertyKeys(0x7F0779B5, 0xFA2B, 0x4766, 0x9C, 0xB2, 0xF7, 0x3B, 0xA3, 0x0B, 0x67, 0x58, 1002);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_CLASS_EXTENSION_SERVICE_REGISTRATION_RESULTS = new WindowsDevicePropertyKeys(0x7F0779B5, 0xFA2B, 0x4766, 0x9C, 0xB2, 0xF7, 0x3B, 0xA3, 0x0B, 0x67, 0x58, 1003);
        public static WindowsDevicePropertyKeys WPD_COMMAND_SERVICE_COMMON_GET_SERVICE_OBJECT_ID = new WindowsDevicePropertyKeys(0x322F071D, 0x36EF, 0x477F, 0xB4, 0xB5, 0x6F, 0x52, 0xD7, 0x34, 0xBA, 0xEE, 2);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_SERVICE_OBJECT_ID = new WindowsDevicePropertyKeys(0x322F071D, 0x36EF, 0x477F, 0xB4, 0xB5, 0x6F, 0x52, 0xD7, 0x34, 0xBA, 0xEE, 1001);
        public static WindowsDevicePropertyKeys WPD_COMMAND_SERVICE_CAPABILITIES_GET_SUPPORTED_METHODS = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 2);
        public static WindowsDevicePropertyKeys WPD_COMMAND_SERVICE_CAPABILITIES_GET_SUPPORTED_METHODS_BY_FORMAT = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 3);
        public static WindowsDevicePropertyKeys WPD_COMMAND_SERVICE_CAPABILITIES_GET_METHOD_ATTRIBUTES = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 4);
        public static WindowsDevicePropertyKeys WPD_COMMAND_SERVICE_CAPABILITIES_GET_METHOD_PARAMETER_ATTRIBUTES = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 5);
        public static WindowsDevicePropertyKeys WPD_COMMAND_SERVICE_CAPABILITIES_GET_SUPPORTED_FORMATS = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 6);
        public static WindowsDevicePropertyKeys WPD_COMMAND_SERVICE_CAPABILITIES_GET_FORMAT_ATTRIBUTES = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 7);
        public static WindowsDevicePropertyKeys WPD_COMMAND_SERVICE_CAPABILITIES_GET_SUPPORTED_FORMAT_PROPERTIES = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 8);
        public static WindowsDevicePropertyKeys WPD_COMMAND_SERVICE_CAPABILITIES_GET_FORMAT_PROPERTY_ATTRIBUTES = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 9);
        public static WindowsDevicePropertyKeys WPD_COMMAND_SERVICE_CAPABILITIES_GET_SUPPORTED_EVENTS = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 10);
        public static WindowsDevicePropertyKeys WPD_COMMAND_SERVICE_CAPABILITIES_GET_EVENT_ATTRIBUTES = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 11);
        public static WindowsDevicePropertyKeys WPD_COMMAND_SERVICE_CAPABILITIES_GET_EVENT_PARAMETER_ATTRIBUTES = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 12);
        public static WindowsDevicePropertyKeys WPD_COMMAND_SERVICE_CAPABILITIES_GET_INHERITED_SERVICES = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 13);
        public static WindowsDevicePropertyKeys WPD_COMMAND_SERVICE_CAPABILITIES_GET_FORMAT_RENDERING_PROFILES = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 14);
        public static WindowsDevicePropertyKeys WPD_COMMAND_SERVICE_CAPABILITIES_GET_SUPPORTED_COMMANDS = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 15);
        public static WindowsDevicePropertyKeys WPD_COMMAND_SERVICE_CAPABILITIES_GET_COMMAND_OPTIONS = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 16);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_SERVICE_CAPABILITIES_SUPPORTED_METHODS = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 1001);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_SERVICE_CAPABILITIES_FORMAT = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 1002);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_SERVICE_CAPABILITIES_METHOD = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 1003);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_SERVICE_CAPABILITIES_METHOD_ATTRIBUTES = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 1004);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_SERVICE_CAPABILITIES_PARAMETER = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 1005);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_SERVICE_CAPABILITIES_PARAMETER_ATTRIBUTES = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 1006);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_SERVICE_CAPABILITIES_FORMATS = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 1007);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_SERVICE_CAPABILITIES_FORMAT_ATTRIBUTES = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 1008);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_SERVICE_CAPABILITIES_PROPERTY_KEYS = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 1009);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_SERVICE_CAPABILITIES_PROPERTY_ATTRIBUTES = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 1010);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_SERVICE_CAPABILITIES_SUPPORTED_EVENTS = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 1011);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_SERVICE_CAPABILITIES_EVENT = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 1012);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_SERVICE_CAPABILITIES_EVENT_ATTRIBUTES = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 1013);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_SERVICE_CAPABILITIES_INHERITANCE_TYPE = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 1014);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_SERVICE_CAPABILITIES_INHERITED_SERVICES = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 1015);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_SERVICE_CAPABILITIES_RENDERING_PROFILES = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 1016);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_SERVICE_CAPABILITIES_SUPPORTED_COMMANDS = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 1017);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_SERVICE_CAPABILITIES_COMMAND = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 1018);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_SERVICE_CAPABILITIES_COMMAND_OPTIONS = new WindowsDevicePropertyKeys(0x24457E74, 0x2E9F, 0x44F9, 0x8C, 0x57, 0x1D, 0x1B, 0xCB, 0x17, 0x0B, 0x89, 1019);
        public static WindowsDevicePropertyKeys WPD_COMMAND_SERVICE_METHODS_START_INVOKE = new WindowsDevicePropertyKeys(0x2D521CA8, 0xC1B0, 0x4268, 0xA3, 0x42, 0xCF, 0x19, 0x32, 0x15, 0x69, 0xBC, 2);
        public static WindowsDevicePropertyKeys WPD_COMMAND_SERVICE_METHODS_CANCEL_INVOKE = new WindowsDevicePropertyKeys(0x2D521CA8, 0xC1B0, 0x4268, 0xA3, 0x42, 0xCF, 0x19, 0x32, 0x15, 0x69, 0xBC, 3);
        public static WindowsDevicePropertyKeys WPD_COMMAND_SERVICE_METHODS_END_INVOKE = new WindowsDevicePropertyKeys(0x2D521CA8, 0xC1B0, 0x4268, 0xA3, 0x42, 0xCF, 0x19, 0x32, 0x15, 0x69, 0xBC, 4);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_SERVICE_METHOD = new WindowsDevicePropertyKeys(0x2D521CA8, 0xC1B0, 0x4268, 0xA3, 0x42, 0xCF, 0x19, 0x32, 0x15, 0x69, 0xBC, 1001);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_SERVICE_METHOD_PARAMETER_VALUES = new WindowsDevicePropertyKeys(0x2D521CA8, 0xC1B0, 0x4268, 0xA3, 0x42, 0xCF, 0x19, 0x32, 0x15, 0x69, 0xBC, 1002);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_SERVICE_METHOD_RESULT_VALUES = new WindowsDevicePropertyKeys(0x2D521CA8, 0xC1B0, 0x4268, 0xA3, 0x42, 0xCF, 0x19, 0x32, 0x15, 0x69, 0xBC, 1003);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_SERVICE_METHOD_CONTEXT = new WindowsDevicePropertyKeys(0x2D521CA8, 0xC1B0, 0x4268, 0xA3, 0x42, 0xCF, 0x19, 0x32, 0x15, 0x69, 0xBC, 1004);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_SERVICE_METHOD_HRESULT = new WindowsDevicePropertyKeys(0x2D521CA8, 0xC1B0, 0x4268, 0xA3, 0x42, 0xCF, 0x19, 0x32, 0x15, 0x69, 0xBC, 1005);
        public static WindowsDevicePropertyKeys SENSOR_EVENT_PARAMETER_EVENT_ID = new WindowsDevicePropertyKeys(0X64346E30, 0X8728, 0X4B34, 0XBD, 0XF6, 0X4F, 0X52, 0X44, 0X2C, 0X5C, 0X28, 2);
        public static WindowsDevicePropertyKeys SENSOR_EVENT_PARAMETER_STATE = new WindowsDevicePropertyKeys(0X64346E30, 0X8728, 0X4B34, 0XBD, 0XF6, 0X4F, 0X52, 0X44, 0X2C, 0X5C, 0X28, 3); // [VT_UI4]
        public static WindowsDevicePropertyKeys SENSOR_PROPERTY_TYPE = new WindowsDevicePropertyKeys(0X7F8383EC, 0XD3EC, 0X495C, 0XA8, 0XCF, 0XB8, 0XBB, 0XE8, 0X5C, 0X29, 0X20, 2); //[VT_CLSID]
        public static WindowsDevicePropertyKeys SENSOR_PROPERTY_STATE = new WindowsDevicePropertyKeys(0X7F8383EC, 0XD3EC, 0X495C, 0XA8, 0XCF, 0XB8, 0XBB, 0XE8, 0X5C, 0X29, 0X20, 3); //[VT_UI4]
        public static WindowsDevicePropertyKeys SENSOR_PROPERTY_PERSISTENT_UNIQUE_ID = new WindowsDevicePropertyKeys(0X7F8383EC, 0XD3EC, 0X495C, 0XA8, 0XCF, 0XB8, 0XBB, 0XE8, 0X5C, 0X29, 0X20, 5); //[VT_CLSID]
        public static WindowsDevicePropertyKeys SENSOR_PROPERTY_MANUFACTURER = new WindowsDevicePropertyKeys(0X7F8383EC, 0XD3EC, 0X495C, 0XA8, 0XCF, 0XB8, 0XBB, 0XE8, 0X5C, 0X29, 0X20, 6); //[VT_LPWSTR]
        public static WindowsDevicePropertyKeys SENSOR_PROPERTY_MODEL = new WindowsDevicePropertyKeys(0X7F8383EC, 0XD3EC, 0X495C, 0XA8, 0XCF, 0XB8, 0XBB, 0XE8, 0X5C, 0X29, 0X20, 7); //[VT_LPWSTR]
        public static WindowsDevicePropertyKeys SENSOR_PROPERTY_SERIAL_NUMBER = new WindowsDevicePropertyKeys(0X7F8383EC, 0XD3EC, 0X495C, 0XA8, 0XCF, 0XB8, 0XBB, 0XE8, 0X5C, 0X29, 0X20, 8); //[VT_LPWSTR]
        public static WindowsDevicePropertyKeys SENSOR_PROPERTY_FRIENDLY_NAME = new WindowsDevicePropertyKeys(0X7F8383EC, 0XD3EC, 0X495C, 0XA8, 0XCF, 0XB8, 0XBB, 0XE8, 0X5C, 0X29, 0X20, 9); //[VT_LPWSTR]
        public static WindowsDevicePropertyKeys SENSOR_PROPERTY_DESCRIPTION = new WindowsDevicePropertyKeys(0X7F8383EC, 0XD3EC, 0X495C, 0XA8, 0XCF, 0XB8, 0XBB, 0XE8, 0X5C, 0X29, 0X20, 10); //[VT_LPWSTR]
        public static WindowsDevicePropertyKeys SENSOR_PROPERTY_CONNECTION_TYPE = new WindowsDevicePropertyKeys(0X7F8383EC, 0XD3EC, 0X495C, 0XA8, 0XCF, 0XB8, 0XBB, 0XE8, 0X5C, 0X29, 0X20, 11); //[VT_UI4]
        public static WindowsDevicePropertyKeys SENSOR_PROPERTY_MIN_REPORT_INTERVAL = new WindowsDevicePropertyKeys(0X7F8383EC, 0XD3EC, 0X495C, 0XA8, 0XCF, 0XB8, 0XBB, 0XE8, 0X5C, 0X29, 0X20, 12); //[VT_UI4]
        public static WindowsDevicePropertyKeys SENSOR_PROPERTY_CURRENT_REPORT_INTERVAL = new WindowsDevicePropertyKeys(0X7F8383EC, 0XD3EC, 0X495C, 0XA8, 0XCF, 0XB8, 0XBB, 0XE8, 0X5C, 0X29, 0X20, 13); //[VT_UI4]
        public static WindowsDevicePropertyKeys SENSOR_PROPERTY_CHANGE_SENSITIVITY = new WindowsDevicePropertyKeys(0X7F8383EC, 0XD3EC, 0X495C, 0XA8, 0XCF, 0XB8, 0XBB, 0XE8, 0X5C, 0X29, 0X20, 14); //[VT_UNKNOWN], IPortableDeviceValues
        public static WindowsDevicePropertyKeys SENSOR_PROPERTY_DEVICE_PATH = new WindowsDevicePropertyKeys(0X7F8383EC, 0XD3EC, 0X495C, 0XA8, 0XCF, 0XB8, 0XBB, 0XE8, 0X5C, 0X29, 0X20, 15); //[VT_LPWSTR]
        public static WindowsDevicePropertyKeys SENSOR_PROPERTY_LIGHT_RESPONSE_CURVE = new WindowsDevicePropertyKeys(0X7F8383EC, 0XD3EC, 0X495C, 0XA8, 0XCF, 0XB8, 0XBB, 0XE8, 0X5C, 0X29, 0X20, 16); //[VT_VECTOR|VT_UI4]
        public static WindowsDevicePropertyKeys SENSOR_PROPERTY_ACCURACY = new WindowsDevicePropertyKeys(0X7F8383EC, 0XD3EC, 0X495C, 0XA8, 0XCF, 0XB8, 0XBB, 0XE8, 0X5C, 0X29, 0X20, 17); //[VT_UNKNOWN], IPortableDeviceValues
        public static WindowsDevicePropertyKeys SENSOR_PROPERTY_RESOLUTION = new WindowsDevicePropertyKeys(0X7F8383EC, 0XD3EC, 0X495C, 0XA8, 0XCF, 0XB8, 0XBB, 0XE8, 0X5C, 0X29, 0X20, 18); //[VT_UNKNOWN], IPortableDeviceValues
        public static WindowsDevicePropertyKeys SENSOR_PROPERTY_LOCATION_DESIRED_ACCURACY = new WindowsDevicePropertyKeys(0X7F8383EC, 0XD3EC, 0X495C, 0XA8, 0XCF, 0XB8, 0XBB, 0XE8, 0X5C, 0X29, 0X20, 19); //[VT_UI4]
        public static WindowsDevicePropertyKeys SENSOR_PROPERTY_RANGE_MINIMUM = new WindowsDevicePropertyKeys(0X7F8383EC, 0XD3EC, 0X495C, 0XA8, 0XCF, 0XB8, 0XBB, 0XE8, 0X5C, 0X29, 0X20, 20); //[VT_UNKNOWN], IPortableDeviceValues
        public static WindowsDevicePropertyKeys SENSOR_PROPERTY_RANGE_MAXIMUM = new WindowsDevicePropertyKeys(0X7F8383EC, 0XD3EC, 0X495C, 0XA8, 0XCF, 0XB8, 0XBB, 0XE8, 0X5C, 0X29, 0X20, 21); //[VT_UNKNOWN], IPortableDeviceValues
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_TIMESTAMP = new WindowsDevicePropertyKeys(0XDB5E0CF2, 0XCF1F, 0X4C18, 0XB4, 0X6C, 0XD8, 0X60, 0X11, 0XD6, 0X21, 0X50, 2); //[VT_FILETIME]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_LATITUDE_DEGREES = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 2); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_LONGITUDE_DEGREES = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 3); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_ALTITUDE_SEALEVEL_METERS = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 4); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_ALTITUDE_ELLIPSOID_METERS = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 5); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_SPEED_KNOTS = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 6); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_TRUE_HEADING_DEGREES = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 7); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_MAGNETIC_HEADING_DEGREES = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 8); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_MAGNETIC_VARIATION = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 9); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_FIX_QUALITY = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 10); //[VT_I4]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_FIX_TYPE = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 11); //[VT_I4]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_POSITION_DILUTION_OF_PRECISION = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 12); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_HORIZONAL_DILUTION_OF_PRECISION = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 13); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_VERTICAL_DILUTION_OF_PRECISION = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 14); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_SATELLITES_USED_COUNT = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 15); //[VT_I4]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_SATELLITES_USED_PRNS = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 16); //[VT_VECTOR | VT_UI4]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_SATELLITES_IN_VIEW = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 17); //[VT_I4]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_SATELLITES_IN_VIEW_PRNS = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 18); //[VT_VECTOR | VT_UI4]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_SATELLITES_IN_VIEW_ELEVATION = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 19); //[VT_VECTOR | VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_SATELLITES_IN_VIEW_AZIMUTH = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 20); //[VT_VECTOR | VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_SATELLITES_IN_VIEW_STN_RATIO = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 21); //[VT_VECTOR | VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_ERROR_RADIUS_METERS = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 22); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_ADDRESS1 = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 23); //[VT_LPWSTR]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_ADDRESS2 = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 24); //[VT_LPWSTR]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_CITY = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 25); //[VT_LPWSTR]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_STATE_PROVINCE = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 26); //[VT_LPWSTR]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_POSTALCODE = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 27); //[VT_LPWSTR]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_COUNTRY_REGION = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 28); //[VT_LPWSTR]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_ALTITUDE_ELLIPSOID_ERROR_METERS = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 29); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_ALTITUDE_SEALEVEL_ERROR_METERS = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 30); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_GPS_SELECTION_MODE = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 31); //[VT_I4]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_GPS_OPERATION_MODE = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 32); //[VT_I4]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_GPS_STATUS = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 33); //[VT_I4]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_GEOIDAL_SEPARATION = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 34); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_DGPS_DATA_AGE = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 35); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_ALTITUDE_ANTENNA_SEALEVEL_METERS = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 36); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_DIFFERENTIAL_REFERENCE_STATION_ID = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 37); //[VT_I4]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_NMEA_SENTENCE = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 38); //[VT_LPWSTR]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_SATELLITES_IN_VIEW_ID = new WindowsDevicePropertyKeys(0X055C74D8, 0XCA6F, 0X47D6, 0X95, 0XC6, 0X1E, 0XD3, 0X63, 0X7A, 0X0F, 0XF4, 39); //[VT_VECTOR|VT_I4]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_TEMPERATURE_CELSIUS = new WindowsDevicePropertyKeys(0X8B0AA2F1, 0X2D57, 0X42EE, 0X8C, 0XC0, 0X4D, 0X27, 0X62, 0X2B, 0X46, 0XC4, 2); //[VT_R4]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_RELATIVE_HUMIDITY_PERCENT = new WindowsDevicePropertyKeys(0X8B0AA2F1, 0X2D57, 0X42EE, 0X8C, 0XC0, 0X4D, 0X27, 0X62, 0X2B, 0X46, 0XC4, 3); //[VT_R4]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_ATMOSPHERIC_PRESSURE_BAR = new WindowsDevicePropertyKeys(0X8B0AA2F1, 0X2D57, 0X42EE, 0X8C, 0XC0, 0X4D, 0X27, 0X62, 0X2B, 0X46, 0XC4, 4); //[VT_R4]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_WIND_DIRECTION_DEGREES_ANTICLOCKWISE = new WindowsDevicePropertyKeys(0X8B0AA2F1, 0X2D57, 0X42EE, 0X8C, 0XC0, 0X4D, 0X27, 0X62, 0X2B, 0X46, 0XC4, 5); //[VT_R4]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_WIND_SPEED_METERS_PER_SECOND = new WindowsDevicePropertyKeys(0X8B0AA2F1, 0X2D57, 0X42EE, 0X8C, 0XC0, 0X4D, 0X27, 0X62, 0X2B, 0X46, 0XC4, 6); //[VT_R4]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_ACCELERATION_X_G = new WindowsDevicePropertyKeys(0X3F8A69A2, 0X7C5, 0X4E48, 0XA9, 0X65, 0XCD, 0X79, 0X7A, 0XAB, 0X56, 0XD5, 2); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_ACCELERATION_Y_G = new WindowsDevicePropertyKeys(0X3F8A69A2, 0X7C5, 0X4E48, 0XA9, 0X65, 0XCD, 0X79, 0X7A, 0XAB, 0X56, 0XD5, 3); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_ACCELERATION_Z_G = new WindowsDevicePropertyKeys(0X3F8A69A2, 0X7C5, 0X4E48, 0XA9, 0X65, 0XCD, 0X79, 0X7A, 0XAB, 0X56, 0XD5, 4); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_ANGULAR_ACCELERATION_X_DEGREES_PER_SECOND_SQUARED = new WindowsDevicePropertyKeys(0X3F8A69A2, 0X7C5, 0X4E48, 0XA9, 0X65, 0XCD, 0X79, 0X7A, 0XAB, 0X56, 0XD5, 5); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_ANGULAR_ACCELERATION_Y_DEGREES_PER_SECOND_SQUARED = new WindowsDevicePropertyKeys(0X3F8A69A2, 0X7C5, 0X4E48, 0XA9, 0X65, 0XCD, 0X79, 0X7A, 0XAB, 0X56, 0XD5, 6); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_ANGULAR_ACCELERATION_Z_DEGREES_PER_SECOND_SQUARED = new WindowsDevicePropertyKeys(0X3F8A69A2, 0X7C5, 0X4E48, 0XA9, 0X65, 0XCD, 0X79, 0X7A, 0XAB, 0X56, 0XD5, 7); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_SPEED_METERS_PER_SECOND = new WindowsDevicePropertyKeys(0X3F8A69A2, 0X7C5, 0X4E48, 0XA9, 0X65, 0XCD, 0X79, 0X7A, 0XAB, 0X56, 0XD5, 8); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_MOTION_STATE = new WindowsDevicePropertyKeys(0X3F8A69A2, 0X7C5, 0X4E48, 0XA9, 0X65, 0XCD, 0X79, 0X7A, 0XAB, 0X56, 0XD5, 9); //[VT_BOOL]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_TILT_X_DEGREES = new WindowsDevicePropertyKeys(0X1637D8A2, 0X4248, 0X4275, 0X86, 0X5D, 0X55, 0X8D, 0XE8, 0X4A, 0XED, 0XFD, 2); //[VT_R4]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_TILT_Y_DEGREES = new WindowsDevicePropertyKeys(0X1637D8A2, 0X4248, 0X4275, 0X86, 0X5D, 0X55, 0X8D, 0XE8, 0X4A, 0XED, 0XFD, 3); //[VT_R4]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_TILT_Z_DEGREES = new WindowsDevicePropertyKeys(0X1637D8A2, 0X4248, 0X4275, 0X86, 0X5D, 0X55, 0X8D, 0XE8, 0X4A, 0XED, 0XFD, 4); //[VT_R4]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_MAGNETIC_HEADING_X_DEGREES = new WindowsDevicePropertyKeys(0X1637D8A2, 0X4248, 0X4275, 0X86, 0X5D, 0X55, 0X8D, 0XE8, 0X4A, 0XED, 0XFD, 5); //[VT_R4]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_MAGNETIC_HEADING_Y_DEGREES = new WindowsDevicePropertyKeys(0X1637D8A2, 0X4248, 0X4275, 0X86, 0X5D, 0X55, 0X8D, 0XE8, 0X4A, 0XED, 0XFD, 6); //[VT_R4]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_MAGNETIC_HEADING_Z_DEGREES = new WindowsDevicePropertyKeys(0X1637D8A2, 0X4248, 0X4275, 0X86, 0X5D, 0X55, 0X8D, 0XE8, 0X4A, 0XED, 0XFD, 7); //[VT_R4]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_DISTANCE_X_METERS = new WindowsDevicePropertyKeys(0X1637D8A2, 0X4248, 0X4275, 0X86, 0X5D, 0X55, 0X8D, 0XE8, 0X4A, 0XED, 0XFD, 8); //[VT_R4]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_DISTANCE_Y_METERS = new WindowsDevicePropertyKeys(0X1637D8A2, 0X4248, 0X4275, 0X86, 0X5D, 0X55, 0X8D, 0XE8, 0X4A, 0XED, 0XFD, 9); //[VT_R4]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_DISTANCE_Z_METERS = new WindowsDevicePropertyKeys(0X1637D8A2, 0X4248, 0X4275, 0X86, 0X5D, 0X55, 0X8D, 0XE8, 0X4A, 0XED, 0XFD, 10); //[VT_R4]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_BOOLEAN_SWITCH_STATE = new WindowsDevicePropertyKeys(0X38564A7C, 0XF2F2, 0X49BB, 0X9B, 0X2B, 0XBA, 0X60, 0XF6, 0X6A, 0X58, 0XDF, 2); //[VT_BOOL]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_MULTIVALUE_SWITCH_STATE = new WindowsDevicePropertyKeys(0X38564A7C, 0XF2F2, 0X49BB, 0X9B, 0X2B, 0XBA, 0X60, 0XF6, 0X6A, 0X58, 0XDF, 3); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_FORCE_NEWTONS = new WindowsDevicePropertyKeys(0X38564A7C, 0XF2F2, 0X49BB, 0X9B, 0X2B, 0XBA, 0X60, 0XF6, 0X6A, 0X58, 0XDF, 4); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_ABSOLUTE_PRESSURE_PASCAL = new WindowsDevicePropertyKeys(0X38564A7C, 0XF2F2, 0X49BB, 0X9B, 0X2B, 0XBA, 0X60, 0XF6, 0X6A, 0X58, 0XDF, 5); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_GAUGE_PRESSURE_PASCAL = new WindowsDevicePropertyKeys(0X38564A7C, 0XF2F2, 0X49BB, 0X9B, 0X2B, 0XBA, 0X60, 0XF6, 0X6A, 0X58, 0XDF, 6); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_STRAIN = new WindowsDevicePropertyKeys(0X38564A7C, 0XF2F2, 0X49BB, 0X9B, 0X2B, 0XBA, 0X60, 0XF6, 0X6A, 0X58, 0XDF, 7); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_WEIGHT_KILOGRAMS = new WindowsDevicePropertyKeys(0X38564A7C, 0XF2F2, 0X49BB, 0X9B, 0X2B, 0XBA, 0X60, 0XF6, 0X6A, 0X58, 0XDF, 8); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_HUMAN_PRESENCE = new WindowsDevicePropertyKeys(0X2299288A, 0X6D9E, 0X4B0B, 0XB7, 0XEC, 0X35, 0X28, 0XF8, 0X9E, 0X40, 0XAF, 2); //[VT_BOOL]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_HUMAN_PROXIMITY_METERS = new WindowsDevicePropertyKeys(0X2299288A, 0X6D9E, 0X4B0B, 0XB7, 0XEC, 0X35, 0X28, 0XF8, 0X9E, 0X40, 0XAF, 3); //[VT_R4]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_TOUCH_STATE = new WindowsDevicePropertyKeys(0X2299288A, 0X6D9E, 0X4B0B, 0XB7, 0XEC, 0X35, 0X28, 0XF8, 0X9E, 0X40, 0XAF, 4); //[VT_BOOL]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_LIGHT_LEVEL_LUX = new WindowsDevicePropertyKeys(0XE4C77CE2, 0XDCB7, 0X46E9, 0X84, 0X39, 0X4F, 0XEC, 0X54, 0X88, 0X33, 0XA6, 2); //[VT_R4]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_LIGHT_TEMPERATURE_KELVIN = new WindowsDevicePropertyKeys(0XE4C77CE2, 0XDCB7, 0X46E9, 0X84, 0X39, 0X4F, 0XEC, 0X54, 0X88, 0X33, 0XA6, 3); //[VT_R4]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_LIGHT_CHROMACITY = new WindowsDevicePropertyKeys(0XE4C77CE2, 0XDCB7, 0X46E9, 0X84, 0X39, 0X4F, 0XEC, 0X54, 0X88, 0X33, 0XA6, 4); //[VT_VECTOR|VT_R4]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_RFID_TAG_40_BIT = new WindowsDevicePropertyKeys(0XD7A59A3C, 0X3421, 0X44AB, 0X8D, 0X3A, 0X9D, 0XE8, 0XAB, 0X6C, 0X4C, 0XAE, 2); //[VT_UI8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_VOLTAGE_VOLTS = new WindowsDevicePropertyKeys(0XBBB246D1, 0XE242, 0X4780, 0XA2, 0XD3, 0XCD, 0XED, 0X84, 0XF3, 0X58, 0X42, 2); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_CURRENT_AMPS = new WindowsDevicePropertyKeys(0XBBB246D1, 0XE242, 0X4780, 0XA2, 0XD3, 0XCD, 0XED, 0X84, 0XF3, 0X58, 0X42, 3); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_CAPACITANCE_FARAD = new WindowsDevicePropertyKeys(0XBBB246D1, 0XE242, 0X4780, 0XA2, 0XD3, 0XCD, 0XED, 0X84, 0XF3, 0X58, 0X42, 4); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_RESISTANCE_OHMS = new WindowsDevicePropertyKeys(0XBBB246D1, 0XE242, 0X4780, 0XA2, 0XD3, 0XCD, 0XED, 0X84, 0XF3, 0X58, 0X42, 5); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_INDUCTANCE_HENRY = new WindowsDevicePropertyKeys(0XBBB246D1, 0XE242, 0X4780, 0XA2, 0XD3, 0XCD, 0XED, 0X84, 0XF3, 0X58, 0X42, 6); //[VT_R8]
        public static WindowsDevicePropertyKeys SENSOR_DATA_TYPE_ELECTRICAL_POWER_WATTS = new WindowsDevicePropertyKeys(0XBBB246D1, 0XE242, 0X4780, 0XA2, 0XD3, 0XCD, 0XED, 0X84, 0XF3, 0X58, 0X42, 7); //[VT_R8]
        public static WindowsDevicePropertyKeys PKEY_PROVIDER_INSTANCEID = new WindowsDevicePropertyKeys(0x84179e61, 0x60f6, 0x4c1c, 0x88, 0xed, 0xf1, 0xc5, 0x31, 0xb3, 0x2b, 0xda, 2);
        public static WindowsDevicePropertyKeys PKEY_PROVIDER_CLSID = new WindowsDevicePropertyKeys(0x84179e61, 0x60f6, 0x4c1c, 0x88, 0xed, 0xf1, 0xc5, 0x31, 0xb3, 0x2b, 0xda, 3);
        public static WindowsDevicePropertyKeys PKEY_PROVIDER_CONFIGUI = new WindowsDevicePropertyKeys(0x84179e61, 0x60f6, 0x4c1c, 0x88, 0xed, 0xf1, 0xc5, 0x31, 0xb3, 0x2b, 0xda, 4);
        public static WindowsDevicePropertyKeys PKEY_PROVIDER_CONTENTTYPE = new WindowsDevicePropertyKeys(0x84179e61, 0x60f6, 0x4c1c, 0x88, 0xed, 0xf1, 0xc5, 0x31, 0xb3, 0x2b, 0xda, 5);
        public static WindowsDevicePropertyKeys PKEY_PROVIDER_CAPABILITIES = new WindowsDevicePropertyKeys(0x84179e61, 0x60f6, 0x4c1c, 0x88, 0xed, 0xf1, 0xc5, 0x31, 0xb3, 0x2b, 0xda, 6);
        public static WindowsDevicePropertyKeys PKEY_PROVIDER_SUPPORTED_ARCHITECTURE = new WindowsDevicePropertyKeys(0x84179e61, 0x60f6, 0x4c1c, 0x88, 0xed, 0xf1, 0xc5, 0x31, 0xb3, 0x2b, 0xda, 7);
        public static WindowsDevicePropertyKeys PKEY_PROVIDER_NAME = new WindowsDevicePropertyKeys(0x84179e61, 0x60f6, 0x4c1c, 0x88, 0xed, 0xf1, 0xc5, 0x31, 0xb3, 0x2b, 0xda, 8);
        public static WindowsDevicePropertyKeys PKEY_PROVIDER_DESCRIPTION = new WindowsDevicePropertyKeys(0x84179e61, 0x60f6, 0x4c1c, 0x88, 0xed, 0xf1, 0xc5, 0x31, 0xb3, 0x2b, 0xda, 9);
        public static WindowsDevicePropertyKeys PKEY_PROVIDER_TOOLTIPS = new WindowsDevicePropertyKeys(0x84179e61, 0x60f6, 0x4c1c, 0x88, 0xed, 0xf1, 0xc5, 0x31, 0xb3, 0x2b, 0xda, 10);
        public static WindowsDevicePropertyKeys PKEY_PROVIDER_ICON = new WindowsDevicePropertyKeys(0x84179e61, 0x60f6, 0x4c1c, 0x88, 0xed, 0xf1, 0xc5, 0x31, 0xb3, 0x2b, 0xda, 11);
        public static WindowsDevicePropertyKeys PKEY_CONFIGUI_INSTANCEID = new WindowsDevicePropertyKeys(0x554b24ea, 0xe8e3, 0x45ba, 0x93, 0x52, 0xdf, 0xb5, 0x61, 0xe1, 0x71, 0xe4, 2);
        public static WindowsDevicePropertyKeys PKEY_CONFIGUI_CLSID = new WindowsDevicePropertyKeys(0x554b24ea, 0xe8e3, 0x45ba, 0x93, 0x52, 0xdf, 0xb5, 0x61, 0xe1, 0x71, 0xe4, 3);
        public static WindowsDevicePropertyKeys PKEY_CONFIGUI_CONTENTTYPE = new WindowsDevicePropertyKeys(0x554b24ea, 0xe8e3, 0x45ba, 0x93, 0x52, 0xdf, 0xb5, 0x61, 0xe1, 0x71, 0xe4, 4);
        public static WindowsDevicePropertyKeys PKEY_CONFIGUI_CAPABILITIES = new WindowsDevicePropertyKeys(0x554b24ea, 0xe8e3, 0x45ba, 0x93, 0x52, 0xdf, 0xb5, 0x61, 0xe1, 0x71, 0xe4, 5);
        public static WindowsDevicePropertyKeys PKEY_CONFIGUI_SUPPORTED_ARCHITECTURE = new WindowsDevicePropertyKeys(0x554b24ea, 0xe8e3, 0x45ba, 0x93, 0x52, 0xdf, 0xb5, 0x61, 0xe1, 0x71, 0xe4, 6);
        public static WindowsDevicePropertyKeys PKEY_CONFIGUI_IS_GLOBAL = new WindowsDevicePropertyKeys(0x554b24ea, 0xe8e3, 0x45ba, 0x93, 0x52, 0xdf, 0xb5, 0x61, 0xe1, 0x71, 0xe4, 7);
        public static WindowsDevicePropertyKeys PKEY_CONFIGUI_NAME = new WindowsDevicePropertyKeys(0x554b24ea, 0xe8e3, 0x45ba, 0x93, 0x52, 0xdf, 0xb5, 0x61, 0xe1, 0x71, 0xe4, 8);
        public static WindowsDevicePropertyKeys PKEY_CONFIGUI_DESCRIPTION = new WindowsDevicePropertyKeys(0x554b24ea, 0xe8e3, 0x45ba, 0x93, 0x52, 0xdf, 0xb5, 0x61, 0xe1, 0x71, 0xe4, 9);
        public static WindowsDevicePropertyKeys PKEY_CONFIGUI_TOOLTIPS = new WindowsDevicePropertyKeys(0x554b24ea, 0xe8e3, 0x45ba, 0x93, 0x52, 0xdf, 0xb5, 0x61, 0xe1, 0x71, 0xe4, 10);
        public static WindowsDevicePropertyKeys PKEY_CONFIGUI_ICON = new WindowsDevicePropertyKeys(0x554b24ea, 0xe8e3, 0x45ba, 0x93, 0x52, 0xdf, 0xb5, 0x61, 0xe1, 0x71, 0xe4, 11);
        public static WindowsDevicePropertyKeys PKEY_CONFIGUI_MENUITEM_NOUI = new WindowsDevicePropertyKeys(0x554b24ea, 0xe8e3, 0x45ba, 0x93, 0x52, 0xdf, 0xb5, 0x61, 0xe1, 0x71, 0xe4, 12);
        public static WindowsDevicePropertyKeys PKEY_CONFIGUI_MENUITEM = new WindowsDevicePropertyKeys(0x554b24ea, 0xe8e3, 0x45ba, 0x93, 0x52, 0xdf, 0xb5, 0x61, 0xe1, 0x71, 0xe4, 13);
        public static WindowsDevicePropertyKeys PKEY_WCN_DeviceType_Category = new WindowsDevicePropertyKeys(0x88190b8b, 0x4684, 0x11da, 0xa2, 0x6a, 0x00, 0x02, 0xb3, 0x98, 0x8e, 0x81, 0x00000010); // VT_UINT
        public static WindowsDevicePropertyKeys PKEY_WCN_DeviceType_SubCategoryOUI = new WindowsDevicePropertyKeys(0x88190b8b, 0x4684, 0x11da, 0xa2, 0x6a, 0x00, 0x02, 0xb3, 0x98, 0x8e, 0x81, 0x00000011); // VT_UINT
        public static WindowsDevicePropertyKeys PKEY_WCN_DeviceType_SubCategory = new WindowsDevicePropertyKeys(0x88190b8b, 0x4684, 0x11da, 0xa2, 0x6a, 0x00, 0x02, 0xb3, 0x98, 0x8e, 0x81, 0x00000012); // VT_UINT
        public static WindowsDevicePropertyKeys PKEY_WCN_SSID = new WindowsDevicePropertyKeys(0x88190b8b, 0x4684, 0x11da, 0xa2, 0x6a, 0x00, 0x02, 0xb3, 0x98, 0x8e, 0x81, 0x00000020); // VT_LPWSTR (should be VT_UI1|VT_VECTOR, but that's not supported by IFunctionInstanceCollectionQuery::AddPropertyConstraint)
        public static WindowsDevicePropertyKeys SIDESHOW_CAPABILITY_DEVICE_ID = new WindowsDevicePropertyKeys(0x8abc88a8, 0x857b, 0x4ad7, 0xa3, 0x5a, 0xb5, 0x94, 0x2f, 0x49, 0x2b, 0x99, 1); // [ VT_LPWSTR ]
        public static WindowsDevicePropertyKeys SIDESHOW_CAPABILITY_SCREEN_TYPE = new WindowsDevicePropertyKeys(0x8abc88a8, 0x857b, 0x4ad7, 0xa3, 0x5a, 0xb5, 0x94, 0x2f, 0x49, 0x2b, 0x99, 2); // [ VT_I4 ]
        public static WindowsDevicePropertyKeys SIDESHOW_CAPABILITY_SCREEN_WIDTH = new WindowsDevicePropertyKeys(0x8abc88a8, 0x857b, 0x4ad7, 0xa3, 0x5a, 0xb5, 0x94, 0x2f, 0x49, 0x2b, 0x99, 3); // [ VT_UI2 ]
        public static WindowsDevicePropertyKeys SIDESHOW_CAPABILITY_SCREEN_HEIGHT = new WindowsDevicePropertyKeys(0x8abc88a8, 0x857b, 0x4ad7, 0xa3, 0x5a, 0xb5, 0x94, 0x2f, 0x49, 0x2b, 0x99, 4); // [ VT_UI2 ]
        public static WindowsDevicePropertyKeys SIDESHOW_CAPABILITY_COLOR_DEPTH = new WindowsDevicePropertyKeys(0x8abc88a8, 0x857b, 0x4ad7, 0xa3, 0x5a, 0xb5, 0x94, 0x2f, 0x49, 0x2b, 0x99, 5); // [ VT_UI2 ]
        public static WindowsDevicePropertyKeys SIDESHOW_CAPABILITY_COLOR_TYPE = new WindowsDevicePropertyKeys(0x8abc88a8, 0x857b, 0x4ad7, 0xa3, 0x5a, 0xb5, 0x94, 0x2f, 0x49, 0x2b, 0x99, 6); // [ VT_I4 ]
        public static WindowsDevicePropertyKeys SIDESHOW_CAPABILITY_DATA_CACHE = new WindowsDevicePropertyKeys(0x8abc88a8, 0x857b, 0x4ad7, 0xa3, 0x5a, 0xb5, 0x94, 0x2f, 0x49, 0x2b, 0x99, 7); // [ VT_BOOL ]
        public static WindowsDevicePropertyKeys SIDESHOW_CAPABILITY_SUPPORTED_LANGUAGES = new WindowsDevicePropertyKeys(0x8abc88a8, 0x857b, 0x4ad7, 0xa3, 0x5a, 0xb5, 0x94, 0x2f, 0x49, 0x2b, 0x99, 8); // [ VT_LPWSTR ]
        public static WindowsDevicePropertyKeys SIDESHOW_CAPABILITY_CURRENT_LANGUAGE = new WindowsDevicePropertyKeys(0x8abc88a8, 0x857b, 0x4ad7, 0xa3, 0x5a, 0xb5, 0x94, 0x2f, 0x49, 0x2b, 0x99, 9); // [ VT_LPWSTR ]
        public static WindowsDevicePropertyKeys SIDESHOW_CAPABILITY_SUPPORTED_THEMES = new WindowsDevicePropertyKeys(0x8abc88a8, 0x857b, 0x4ad7, 0xa3, 0x5a, 0xb5, 0x94, 0x2f, 0x49, 0x2b, 0x99, 10);// [ VT_LPWSTR ]
        public static WindowsDevicePropertyKeys SIDESHOW_CAPABILITY_SUPPORTED_IMAGE_FORMATS = new WindowsDevicePropertyKeys(0x8abc88a8, 0x857b, 0x4ad7, 0xa3, 0x5a, 0xb5, 0x94, 0x2f, 0x49, 0x2b, 0x99, 14);// [ VT_LPWSTR ]
        public static WindowsDevicePropertyKeys SIDESHOW_CAPABILITY_CLIENT_AREA_WIDTH = new WindowsDevicePropertyKeys(0x8abc88a8, 0x857b, 0x4ad7, 0xa3, 0x5a, 0xb5, 0x94, 0x2f, 0x49, 0x2b, 0x99, 15);// [ VT_UI2 ]
        public static WindowsDevicePropertyKeys SIDESHOW_CAPABILITY_CLIENT_AREA_HEIGHT = new WindowsDevicePropertyKeys(0x8abc88a8, 0x857b, 0x4ad7, 0xa3, 0x5a, 0xb5, 0x94, 0x2f, 0x49, 0x2b, 0x99, 16);// [ VT_UI2 ]
        public static WindowsDevicePropertyKeys WPD_COMMAND_MTP_EXT_GET_SUPPORTED_VENDOR_OPCODES = new WindowsDevicePropertyKeys(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56, 11);
        public static WindowsDevicePropertyKeys WPD_COMMAND_MTP_EXT_EXECUTE_COMMAND_WITHOUT_DATA_PHASE = new WindowsDevicePropertyKeys(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56, 12);
        public static WindowsDevicePropertyKeys WPD_COMMAND_MTP_EXT_EXECUTE_COMMAND_WITH_DATA_TO_READ = new WindowsDevicePropertyKeys(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56, 13);
        public static WindowsDevicePropertyKeys WPD_COMMAND_MTP_EXT_EXECUTE_COMMAND_WITH_DATA_TO_WRITE = new WindowsDevicePropertyKeys(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56, 14);
        public static WindowsDevicePropertyKeys WPD_COMMAND_MTP_EXT_READ_DATA = new WindowsDevicePropertyKeys(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56, 15);
        public static WindowsDevicePropertyKeys WPD_COMMAND_MTP_EXT_WRITE_DATA = new WindowsDevicePropertyKeys(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56, 16);
        public static WindowsDevicePropertyKeys WPD_COMMAND_MTP_EXT_END_DATA_TRANSFER = new WindowsDevicePropertyKeys(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56, 17);
        public static WindowsDevicePropertyKeys WPD_COMMAND_MTP_EXT_GET_VENDOR_EXTENSION_DESCRIPTION = new WindowsDevicePropertyKeys(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56, 18);
        public static WindowsDevicePropertyKeys WPD_PROPERTY_MTP_EXT_OPERATION_CODE = new WindowsDevicePropertyKeys(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56, 1001);    // [ VT_UI4 ] : Input param which identifies the vendor-extended MTP operation code
        public static WindowsDevicePropertyKeys WPD_PROPERTY_MTP_EXT_OPERATION_PARAMS = new WindowsDevicePropertyKeys(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56, 1002);    // [ VT_UNKNOWN ] : Input IPortableDevicePropVariantCollection (of VT_UI4) specifying the params for the vendor operation
        public static WindowsDevicePropertyKeys WPD_PROPERTY_MTP_EXT_RESPONSE_CODE = new WindowsDevicePropertyKeys(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56, 1003);    // [ VT_UI4 ] : Output param which identifies the response code for the vendor operation
        public static WindowsDevicePropertyKeys WPD_PROPERTY_MTP_EXT_RESPONSE_PARAMS = new WindowsDevicePropertyKeys(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56, 1004);    // [ VT_UNKNOWN ] : Returns an IPortableDevicePropVariantCollection (of VT_UI4) of response params for the vendor operation
        public static WindowsDevicePropertyKeys WPD_PROPERTY_MTP_EXT_VENDOR_OPERATION_CODES = new WindowsDevicePropertyKeys(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56, 1005);    // [ VT_UNKNOWN ] : Returns an IPortableDevicePropVariantCollection (of VT_UI4) of Vendor-extended MTP codes
        public static WindowsDevicePropertyKeys WPD_PROPERTY_MTP_EXT_TRANSFER_CONTEXT = new WindowsDevicePropertyKeys(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56, 1006);    // [ VT_LPWSTR ] : Returned as a context idetifier (a string value) for subsequent data transfer
        public static WindowsDevicePropertyKeys WPD_PROPERTY_MTP_EXT_TRANSFER_TOTAL_DATA_SIZE = new WindowsDevicePropertyKeys(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56, 1007);    // [ VT_UI8 ] : Input (when writing data) or output (when reading data) param which specifies total data size in bytes (excluding any overhead)
        public static WindowsDevicePropertyKeys WPD_PROPERTY_MTP_EXT_TRANSFER_NUM_BYTES_TO_READ = new WindowsDevicePropertyKeys(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56, 1008); // [ VT_UI4 ] : Input param specifying the number of bytes to read from device in a series of read calls
        public static WindowsDevicePropertyKeys WPD_PROPERTY_MTP_EXT_TRANSFER_NUM_BYTES_READ = new WindowsDevicePropertyKeys(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56, 1009); // [ VT_UI4 ] : Output param specifying the actual number of bytes (no overhead) received from device in a read call
        public static WindowsDevicePropertyKeys WPD_PROPERTY_MTP_EXT_TRANSFER_NUM_BYTES_TO_WRITE = new WindowsDevicePropertyKeys(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56, 1010); // [ VT_UI4 ] : Input specifying the number of bytes to send to device in a series of write calls
        public static WindowsDevicePropertyKeys WPD_PROPERTY_MTP_EXT_TRANSFER_NUM_BYTES_WRITTEN = new WindowsDevicePropertyKeys(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56, 1011); // [ VT_UI4 ] : Returns the actual number of bytes (no overhead) sent to device in a write call
        public static WindowsDevicePropertyKeys WPD_PROPERTY_MTP_EXT_TRANSFER_DATA = new WindowsDevicePropertyKeys(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56, 1012); // [ VT_VECTOR|VT_UI1 ] : Stores the binary data to transfer from/to device
        public static WindowsDevicePropertyKeys WPD_PROPERTY_MTP_EXT_OPTIMAL_TRANSFER_BUFFER_SIZE = new WindowsDevicePropertyKeys(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56, 1013); // [ VT_UI4 ] : Returns the optimal size of the transfer buffer
        public static WindowsDevicePropertyKeys WPD_PROPERTY_MTP_EXT_VENDOR_EXTENSION_DESCRIPTION = new WindowsDevicePropertyKeys(0x4d545058, 0x1a2e, 0x4106, 0xa3, 0x57, 0x77, 0x1e, 0x8, 0x19, 0xfc, 0x56, 1014); // [ VT_LPWSTR ] : Returns vendor extension description string
        public static WindowsDevicePropertyKeys WPD_PROPERTY_MTP_EXT_EVENT_PARAMS = new WindowsDevicePropertyKeys(0x4d545058, 0xef88, 0x4e4d, 0x95, 0xc3, 0x4f, 0x32, 0x7f, 0x72, 0x8a, 0x96, 1011);    // [ VT_UNKNOWN ] : Returns an IPortableDevicePropVariantCollection (of VT_UI4) of event params for a vendor-extended event
        public static WindowsDevicePropertyKeys WPDNSE_OBJECT_HAS_CONTACT_PHOTO = new WindowsDevicePropertyKeys(0x34d71409, 0x4b47, 0x4d80, 0xaa, 0xac, 0x3a, 0x28, 0xa4, 0xa3, 0xb3, 0xe6, 2); // [ VT_BOOL ] Indicates whether the object has a contact photo resource.  
        public static WindowsDevicePropertyKeys WPDNSE_OBJECT_HAS_THUMBNAIL = new WindowsDevicePropertyKeys(0x34d71409, 0x4b47, 0x4d80, 0xaa, 0xac, 0x3a, 0x28, 0xa4, 0xa3, 0xb3, 0xe6, 3); // [ VT_BOOL ] Indicates whether the object has a thumbnail resource.  
        public static WindowsDevicePropertyKeys WPDNSE_OBJECT_HAS_ICON = new WindowsDevicePropertyKeys(0x34d71409, 0x4b47, 0x4d80, 0xaa, 0xac, 0x3a, 0x28, 0xa4, 0xa3, 0xb3, 0xe6, 4); // [ VT_BOOL ] Indicates whether the object has an icon resource.  
        public static WindowsDevicePropertyKeys WPDNSE_OBJECT_HAS_AUDIO_CLIP = new WindowsDevicePropertyKeys(0x34d71409, 0x4b47, 0x4d80, 0xaa, 0xac, 0x3a, 0x28, 0xa4, 0xa3, 0xb3, 0xe6, 5); // [ VT_BOOL ] Indicates whether the object has a voice annotation resource.  
        public static WindowsDevicePropertyKeys WPDNSE_OBJECT_HAS_ALBUM_ART = new WindowsDevicePropertyKeys(0x34d71409, 0x4b47, 0x4d80, 0xaa, 0xac, 0x3a, 0x28, 0xa4, 0xa3, 0xb3, 0xe6, 6); // [ VT_BOOL ] Indicates whether the object has an album art resource.  
        public static WindowsDevicePropertyKeys WPDNSE_OBJECT_OPTIMAL_READ_BLOCK_SIZE = new WindowsDevicePropertyKeys(0x34d71409, 0x4b47, 0x4d80, 0xaa, 0xac, 0x3a, 0x28, 0xa4, 0xa3, 0xb3, 0xe6, 7); // [ VT_UI4 ] The optimal buffer size clients can use to read data chunks of the default resource.  
        public static WindowsDevicePropertyKeys PKEY_AudioEndPoint_Interface = new WindowsDevicePropertyKeys("a45c254e-df1c-4efd-8020-67d146a850e0", 2);
        public static WindowsDevicePropertyKeys PKEY_AudioEndPoint_DisableSysFx = new WindowsDevicePropertyKeys("1da5d803-d492-4edd-8c23-e0c0ffee7f0e", 5);
        public static WindowsDevicePropertyKeys PKEY_Endpoint_Enable_LFX = new WindowsDevicePropertyKeys("a988f78b-07b6-4f47-9c9f-25409534cdee", 0);
        public static WindowsDevicePropertyKeys PKEY_Endpoint_Enable_LFX_BEAMFORMING = new WindowsDevicePropertyKeys("818d3b4c-2bbf-40e9-a438-9361b0ffc427", 0);
        public static WindowsDevicePropertyKeys PKEY_Endpoint_Enable_LFX_AEC = new WindowsDevicePropertyKeys("7996efb2-29c1-4898-be83-4a3646e06fac", 0);
        public static WindowsDevicePropertyKeys PKEY_Endpoint_Enable_GFX = new WindowsDevicePropertyKeys("7f73d4e1-91e3-4490-9fba-5e86680a5748", 0);
        public static WindowsDevicePropertyKeys PKEY_SupportFormat_OEMFormat = new WindowsDevicePropertyKeys("b3f8fa53-0004-438e-9003-51a46e139bfc", 5);
        public static WindowsDevicePropertyKeys PKEY_Endpoint_Disable_AC3 = new WindowsDevicePropertyKeys("b3f8fa53-0004-438e-9003-51a46e139bfc", 5);
        public static WindowsDevicePropertyKeys PKEY_Endpoint_Name = new WindowsDevicePropertyKeys("b3f8fa53-0004-438e-9003-51a46e139bfc", 6);
        public static WindowsDevicePropertyKeys PKEY_AudioEndPoint_Type = new WindowsDevicePropertyKeys("1da5d803-d492-4edd-8c23-e0c0ffee7f0e", 0);

        public static WindowsDevicePropertyKeys PKEY_SYSFX_Association = new WindowsDevicePropertyKeys("{D04E05A6-594B-4FB6-A80D-01AF5EED7D1D}", 0);
        public static WindowsDevicePropertyKeys PKEY_SYSFX_PreMixClsid = new WindowsDevicePropertyKeys("{D04E05A6-594B-4FB6-A80D-01AF5EED7D1D}", 1);
        public static WindowsDevicePropertyKeys PKEY_SYSFX_PostMixClsid = new WindowsDevicePropertyKeys("{D04E05A6-594B-4FB6-A80D-01AF5EED7D1D}", 2);
        public static WindowsDevicePropertyKeys PKEY_SYSFX_UiClsid = new WindowsDevicePropertyKeys("{D04E05A6-594B-4FB6-A80D-01AF5EED7D1D}", 3);
        public static WindowsDevicePropertyKeys PKEY_DisplayName = new WindowsDevicePropertyKeys("{B725F130-47EF-101A-A5F1-02608C9EEBAC}", 10);
        public static WindowsDevicePropertyKeys PKEY_AudioEndpoint_Ext_UiClsid = new WindowsDevicePropertyKeys("{1DA5D803-D492-4EDD-8C23-E0C0FFEE7F0E}", 1);
        public static WindowsDevicePropertyKeys Render_AudioEndpoint_Flag = new WindowsDevicePropertyKeys("b3f8fa53-0004-438e-9003-51a46e139bfc", 3);
        public static WindowsDevicePropertyKeys Render_AudioEndpoint_Flag2 = new WindowsDevicePropertyKeys("b3f8fa53-0004-438e-9003-51a46e139bfc", 4);
        #endregion Device Properties
    }
    /// <summary>
    /// audio formats likely in system.audio
    /// </summary>
    public static class AudioFormats
    {
        public static Guid FMTID_AudioSummaryInformation = new Guid("64440490-4C8B-11D1-8B70-080036B11A03");
        public static Guid FMTID_IsVariableBitRate = new Guid("E6822FEE-8C17-4D62-823C-8E9CFCBD1D5C");
        public static Guid FMTID_AudioPeakValue = new Guid("2579E5D0-1116-4084-BD9A-9B4F7CB4DF5E");
        public static Guid FMTID_SummaryInformation = new Guid("F29F85E0-4FF9-1068-AB91-08002B27B3D9");
        public static Guid PSGUID_MEDIAFILESUMMARYINFORMATION = new Guid("64440492-4C8B-11D1-8B70-080036B11A03");
        public static Guid FMTID_DRM = new Guid("AEAC19E4-89AE-4508-B9B7-BB867ABEE2ED");
        public static Guid FMTID_DocumentSummaryInformation = new Guid("D5CDD502-2E9C-101B-9397-08002B2CF9AE");
        public static Guid FMTID_Media_AverageLevel = new Guid("09EDD5B6-B301-43C5-9990-D00302EFFD46");
        public static Guid FMTID_Media_DateEncoded = new Guid("2E4B640D-5019-46D8-8881-55414CC5CAA0");
        public static Guid FMTID_Media_DateReleased = new Guid("DE41CC29-6971-4290-B472-F59F2E2F31E2");
        public static Guid FMTID_MUSIC = new Guid("56A3372E-CE9C-11D2-9F0E-006097C686F6");
        public static Guid FMTID_Music_IsCompilation = new Guid("C449D5CB-9EA4-4809-82E8-AF9D59DED6D1");
        public static Guid FMTID_ParentalRatingReason = new Guid("10984E0A-F9F2-4321-B7EF-BAF195AF4319");
        public static Guid FMTID_VideoSummaryInformation = new Guid("64440491-4C8B-11D1-8B70-080036B11A03");
        public static Guid FMTID_Endpoint_Info = new Guid("a45c254e-df1c-4efd-8020-67d146a850e0");

        public static Guid FMTID_cantfind2 = new Guid("233164c8-1b2c-4c7d-bc68-b671687a2567");

    }
    static class PropertyKeyExtensions
    {
        /// <summary>
        /// returns the name of a specified guid type.
        /// (if guid = new Guid(0x73646976, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71))
        /// typeof(directshow.mediatype).InterpretGuidType(guid) returns: "Video")
        /// </summary>
        /// <param name="valueParentClassType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string InterpretType<T>(this Type valueParentClassType, T value)
        {
            return valueParentClassType.InterpretType(value, value.ToString());
        }
        /// <summary>
        /// returns the name of a specified guid type.
        /// (if guid = new Guid(0x73646976, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71))
        /// typeof(directshow.mediatype).InterpretGuidType(guid) returns: "Video")
        /// </summary>
        /// <param name="valueParentClassType"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string InterpretType<T>(this Type valueParentClassType, T value, string defaultValue = "")
        {
            KeyValuePair<string, T>[] list = ListTypes<T>(valueParentClassType);
            for (int i = 0; i < list.Length; i++)
                if (object.Equals(list[i].Value, value))
                    return list[i].Key;
            return defaultValue;
        }
        /// <summary>
        /// returns the names of the static guid fields of a class.
        /// (directshow.mediatype.ListGuidTypes() returns: "Null", "Video", "Interleaved", "Audio", ...)
        /// </summary>
        /// <param name="classType"></param>
        /// <returns></returns>
        public static KeyValuePair<string, T>[] ListTypes<T>(this Type classType)
        {
            List<KeyValuePair<string, T>> list = new List<KeyValuePair<string, T>>();
            foreach (FieldInfo fi in classType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
                if (fi.FieldType == typeof(T))
                    list.Add(new KeyValuePair<string, T>(fi.Name, (T)fi.GetValue(classType)));
            return list.ToArray();
        }
        /// <summary>
        /// returns a set of properties associated with a type
        /// that have attributes of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetProperties<T>(this Type type) where T : Attribute
        {
            List<PropertyInfo> ret = new List<PropertyInfo>();
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;// | BindingFlags.FlattenHierarchy;
            foreach (PropertyInfo pi in type.GetProperties(flags))
            {
                foreach (object oat in pi.GetCustomAttributes(typeof(T), false))
                {
                    T item = (T)oat;
                    ret.Add(pi);
                }
            }
            return ret.ToArray();
        }
        /// <summary>
        /// returns a set of properties that contain a specific type of attribute
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetTypedProperties<T>(this object o) where T : Attribute
        {
            return o == null ? new List<PropertyInfo>().ToArray() : o.GetType().GetProperties<T>();
        }
    }
    public enum AudioChannelCounts
    {
        Mono = 1,
        Stereo = 2
    }
    public enum CalendarShowTimeAsValues
    {
        Free = 0,
        Tentative = 1,
        Busy = 2,
        OOF = 3
    }
    public enum CommunicationTaskStatuses
    {
        NotStarted = 0,
        InProgress = 1,
        Complete = 2,
        Waiting = 3,
        Deferred = 4
    }
    public enum FlagColors
    {
        Purple = 1,
        Orange = 2,
        Green = 3,
        Yellow = 4,
        Blue = 5,
        Red = 6
    }
    public enum FlagStatuses
    {
        NotFlagged = 0,
        Completed = 1,
        FollowUp = 2
    }
    public enum Importances
    {
        LowMin = 0,
        LowSet = 1,
        LowMax = 1,
        NormalMin = 2,
        NormalSet = 3,
        NormalMax = 4,
        HighMin = 5,
        HighSet = 5,
        HighMax = 5
    }
    public class Kinds
    {
        public const string Calendar = "calendar";
        public const string Communication = "communication";
        public const string Contact = "contact";
        public const string Document = "document";
        public const string EMail = "email";
        public const string Feed = "feed";
        public const string Folder = "folder";
        public const string Game = "game";
        public const string InstantMessage = "instantmessage";
        public const string Journal = "journal";
        public const string Link = "link";
        public const string Movie = "movie";
        public const string Music = "music";
        public const string Note = "note";
        public const string Picture = "picture";
        public const string Program = "program";
        public const string RecordedTV = "recordedtv";
        public const string SearchFolder = "searchfolder";
        public const string Task = "task";
        public const string Video = "video";
        public const string WebHistory = "webhistory";
    }
    public enum OfflineAvailabilities
    {
        NotAvailable = 0,
        Available = 1,
        AlwaysAvailable = 2
    }
    public enum OfflineStatuses
    {
        Online = 0,
        Offline = 1,
        OfflineForced = 2,
        OfflineSlow = 3,
        OfflineError = 4,
        OfflineItemVersionConflict = 5,
        OfflineSuspended = 6
    }
    public enum Priorities
    {
        Low = 0,
        Normal = 1,
        High = 2
    }
    public enum Ratings
    {
        UnratedMin = 0,
        UnratedSet = 0,
        UnratedMax = 0,
        OneStarMin = 1,
        OneStarSet = 1,
        OneStarMax = 12,
        TwoStarsMin = 13,
        TwoStarsSet = 25,
        TwoStarsMax = 37,
        ThreeStarsMin = 38,
        ThreeStarsSet = 50,
        ThreeStarsMax = 62,
        FourStarsMin = 63,
        FourStarsSet = 75,
        FourStarsMax = 87,
        FiveStarsMin = 88,
        FiveStarsSet = 99,
        FiveStarsMax = 99
    }
    public enum Sensitivities
    {
        Normal = 0,
        Personal = 1,
        Private = 2,
        Confidential = 3
    }
    public enum ColorSpaces
    {
        SRGB = 1,
        Uncalubrated = 0xffff
    }
    public enum ImageCompressions
    {
        Uncomressed = 1,
        CCITT_T3 = 2,
        CCITT_T4 = 3,
        CCITT_T6 = 4,
        LZW = 5,
        JPEG = 6,
        PackBits = 32773
    }
    public enum LinkStatuses
    {
        Resolved = 1,
        Broken = 2
    }
    public enum NoteColors
    {
        Blue = 0,
        Green = 1,
        Pink = 2,
        Yellow = 3,
        White = 4,
        LightGreen = 5
    }
    public enum PhotoContrasts
    {
        Normal = 0,
        Soft = 1,
        Hard = 2
    }
    public enum PhotoExposurePrograms
    {
        Unknown = 0,
        Manual = 1,
        Normal = 2,
        Aperture = 3,
        Shutter = 4,
        Creative = 5,
        Action = 6,
        Portrait = 7,
        Landscape = 8
    }
    public enum PhotoFlashes
    {
        None = 0,
        Flast = 1,
        WithoutStrobe = 5,
        WithStrobe = 7
    }
    public enum PhotoGainControls
    {
        None = 0,
        LowGainUp = 1,
        HighGainUp = 2,
        LowGainDown = 3,
        HighGainDown = 4
    }
    public enum PhotoLightSources
    {
        Unknown = 0,
        DayLight = 1,
        Fluorescent = 2,
        Tungsten = 3,
        StandardA = 17,
        StandardB = 18,
        StandardC = 19,
        D55 = 20,
        D65 = 21,
        D75 = 22
    }
    public enum PhotoMeteringModes
    {
        Unknown = 0,
        Average = 1,
        Center = 2,
        Spot = 3,
        MultiSpot = 4,
        Pattern = 5,
        Partial = 6
    }
    public enum PhotoOrientations
    {
        Normal = 1,
        FlipHorizontal = 2,
        Rotate180 = 3,
        FlipVertical = 4,
        Transpose = 5,
        Rotate270 = 6,
        Transverse = 7,
        Rotate90 = 8
    }
    public enum PhotoPhotometricInterpretations
    {
        RGB = 2,
        YCBCR = 6
    }
    public enum PhotoProgramModes
    {
        NotDefined = 0,
        Manual = 1,
        Normal = 2,
        Aperture = 3,
        Shutter = 4,
        Creative = 5,
        Action = 6,
        Portrait = 7,
        Landscape = 8
    }
    public enum PhotoSaturations
    {
        Normal = 0,
        Low = 1,
        High = 2
    }
    public enum PhotoSharpnesses
    {
        Normal = 0,
        Soft = 1,
        Hard = 2
    }
    public enum PhotoWhiteBalances
    {
        Auto = 0,
        Manual = 1
    }
    public class ShellSFGAOFlagsStrings
    {
        public const string SFGAOSTR_FILESYS = "filesys";    // SFGAO_FILESYSTEM
        public const string SFGAOSTR_FILEANC = "fileanc";    // SFGAO_FILESYSANCESTOR
        public const string SFGAOSTR_STORAGEANC = "storageanc";  // SFGAO_STORAGEANCESTOR
        public const string SFGAOSTR_STREAM = "stream";      // SFGAO_STREAM
        public const string SFGAOSTR_LINK = "link";          // SFGAO_LINK
        public const string SFGAOSTR_HIDDEN = "hidden";      // SFGAO_HIDDEN
        public const string SFGAOSTR_FOLDER = "folder";      // SFGAO_FOLDER
        public const string SFGAOSTR_NONENUM = "nonenum";    // SFGAO_NONENUMERATED
        public const string SFGAOSTR_BROWSABLE = "browsable";    // SFGAO_BROWSABLE    
    }
    public enum SyncHandlerTypes
    {
        Other = 0,
        Programs = 1,
        Devices = 2,
        Folders = 3,
        WebServices = 4,
        Computers = 5
    }
}