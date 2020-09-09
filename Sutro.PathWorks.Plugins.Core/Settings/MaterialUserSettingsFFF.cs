﻿using Sutro.Core.Settings.Material;
using Sutro.PathWorks.Plugins.API.Settings;
using Sutro.PathWorks.Plugins.Core.Translations;
using Sutro.PathWorks.Plugins.Core.UserSettings;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Sutro.PathWorks.Plugins.Core.Settings
{
    [SuppressMessage("NDepend", "ND1000:AvoidTypesTooBig", Justification = "...")]
    [SuppressMessage("NDepend", "ND1002:AvoidTypesWithTooManyFields", Justification = "...")]
    public class MaterialUserSettingsFFF<TSettings> : UserSettingCollectionBase<TSettings> where TSettings : MaterialProfileFFF
    {
        #region Identifiers

        public static readonly UserSettingGroup GroupIdentifiers =
            new UserSettingGroup(() => UserSettingTranslations.GroupMaterialIdentifiers);

        public UserSettingString<TSettings> MaterialName { get; } = new UserSettingString<TSettings>(
            "MaterialUserSettingsFFF.Name",
            () => UserSettingTranslations.Name_Name,
            () => UserSettingTranslations.Name_Description,
            GroupIdentifiers,
            (settings) => settings.Name,
            (settings, val) => settings.Name = val);

        public UserSettingString<TSettings> MaterialSource { get; } = new UserSettingString<TSettings>(
            "MaterialUserSettingsFFF.MaterialSource",
            () => UserSettingTranslations.MaterialSource_Name,
            () => UserSettingTranslations.MaterialSource_Description,
            GroupIdentifiers,
            (settings) => settings.Supplier,
            (settings, val) => settings.Supplier = val);

        public UserSettingString<TSettings> MaterialType { get; } = new UserSettingString<TSettings>(
            "MaterialUserSettingsFFF.MaterialType",
            () => UserSettingTranslations.MaterialType_Name,
            () => UserSettingTranslations.MaterialType_Description,
            GroupIdentifiers,
            (settings) => settings.Supplier,
            (settings, val) => settings.Supplier = val);

        public UserSettingString<TSettings> MaterialColor { get; } = new UserSettingString<TSettings>(
            "MaterialUserSettingsFFF.MaterialColor",
            () => UserSettingTranslations.MaterialColor_Name,
            () => UserSettingTranslations.MaterialColor_Description,
            GroupIdentifiers,
            (settings) => settings.Color,
            (settings, val) => settings.Color = val);

        #endregion Identifiers

        #region Basic

        public static readonly UserSettingGroup GroupBasic =
            new UserSettingGroup(() => UserSettingTranslations.GroupBasic);

        public UserSettingDouble<TSettings> FilamentDiamMM { get; } = new UserSettingDouble<TSettings>(
            "MaterialUserSettingsFFF.FilamentDiamMM",
            () => UserSettingTranslations.FilamentDiamMM_Name,
            () => UserSettingTranslations.FilamentDiamMM_Description,
            GroupBasic,
            (settings) => settings.FilamentDiamMM,
            (settings, val) => settings.FilamentDiamMM = val,
            minimumValue: 0,
            unitsF: () => UserSettingTranslations.Units_Millimeters);

        #endregion Basic

        #region Temperature

        public static readonly UserSettingGroup GroupTemperature =
            new UserSettingGroup(() => UserSettingTranslations.GroupTemperature);

        public UserSettingInt<TSettings> ExtruderTempC { get; } = new UserSettingInt<TSettings>(
            "MaterialUserSettingsFFF.FilamentDiamMM",
            () => UserSettingTranslations.ExtruderTempC_Name,
            () => UserSettingTranslations.ExtruderTempC_Description,
            GroupTemperature,
            (settings) => settings.ExtruderTempC,
            (settings, val) => settings.ExtruderTempC = val,
            minimumValue: -273,
            unitsF: () => UserSettingTranslations.Units_DegreesCelsius);

        public UserSettingInt<TSettings> HeatedBedTempC { get; } = new UserSettingInt<TSettings>(
            "MaterialUserSettingsFFF.HeatedBedTempC",
            () => UserSettingTranslations.HeatedBedTempC_Name,
            () => UserSettingTranslations.HeatedBedTempC_Description,
            GroupTemperature,
            (settings) => settings.HeatedBedTempC,
            (settings, val) => settings.HeatedBedTempC = val,
            minimumValue: -273,
            unitsF: () => UserSettingTranslations.Units_DegreesCelsius);

        #endregion Temperature

        /// <summary>
        /// Sets the culture for name & description strings.
        /// </summary>
        /// <remarks>
        /// Any translation resources used in derived classes should override to set the culture
        /// on the resource manager, while still calling the base method.
        /// </remarks>
        /// <param name="cultureInfo"></param>
        public override void SetCulture(CultureInfo cultureInfo)
        {
            UserSettingTranslations.Culture = cultureInfo;
        }
    }
}