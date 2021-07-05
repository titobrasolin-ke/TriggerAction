using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace TriggerAction.ServiceModel.Types
{
    [DataContract]
    public partial class Template
    {
        [DataMember(Name="UrbanDataset")]
        public UrbanDataset UrbanDataset { get; set; }
    }

    [DataContract]
    public partial class UrbanDataset
    {
        /// <value>
        /// The <c>Specification</c> section contains the information that describes the UrbanDataset, such as: the
        /// version of the specifications and the list of properties that are represented by it.
        /// See <a href="http://smartcityplatform.enea.it/#/en/specification/information/2.0/index.html#specificationsection">SCPS Information 2.0</a>.
        /// </value>
        [DataMember(Name="specification")]
        public Specification Specification { get; set; }

        /// <value>
        /// The <c>Context</c> section allows you to contextualize the UrbanDataset (for example via a time stamp and
        /// geographic coordinates).
        /// See: <a href="http://smartcityplatform.enea.it/#/en/specification/information/2.0/index.html#contextsection">SCPS Information 2.0</a>.
        /// </value>
        [DataMember(Name="context")]
        public Context Context { get; set; }

        /// <value>
        /// The <c>Values</c> section is composed of one or more rows (1 row = 1 group of properties), each of which
        /// contains the name and value of one of the data that is represented by the UrbanDataset.
        /// See: <a href="http://smartcityplatform.enea.it/#/en/specification/information/2.0/index.html#valuessection">SCPS Information 2.0</a>.
        /// </value>
        [DataMember(Name="values")]
        public Values Values { get; set; }
    }

    [DataContract]
    public partial class Context
    {
        [DataMember(Name="producer")]
        public Producer Producer { get; set; }

        [DataMember(Name="timeZone")]
        public string TimeZone { get; set; }

        [DataMember(Name="timestamp")]
        public DateTime Timestamp { get; set; }

        [DataMember(Name="coordinates")]
        public Coordinates Coordinates { get; set; }

        [DataMember(Name="language")]
        public string Language { get; set; }
    }

    [DataContract]
    public partial class Coordinates
    {
        [DataMember(Name="format")]
        public string Format { get; set; }

        [DataMember(Name="latitude")]
        public double Latitude { get; set; }

        [DataMember(Name="longitude")]
        public double Longitude { get; set; }

        [DataMember(Name="height")]
        public double? Height { get; set; }
    }

    [DataContract]
    public partial class Producer
    {
        [DataMember(Name="id")]
        public string Id { get; set; }

        [DataMember(Name="schemeID")]
        public string SchemeId { get; set; }
    }

    [DataContract]
    public partial class Specification
    {
        [DataMember(Name="version")]
        public string Version { get; set; }

        [DataMember(Name="id")]
        public Id Id { get; set; }

        [DataMember(Name="name")]
        public string Name { get; set; }

        [DataMember(Name="uri")]
        public Uri Uri { get; set; }

        [DataMember(Name="properties")]
        public Properties Properties { get; set; }
    }

    [DataContract]
    public partial class Id
    {
        [DataMember(Name="value")]
        public string Value { get; set; }

        [DataMember(Name="schemeID")]
        public string SchemeId { get; set; }
    }

    [DataContract]
    public partial class Properties
    {
        [DataMember(Name="propertyDefinition")]
        public List<PropertyDefinition> PropertyDefinition { get; set; }
    }

    [DataContract]
    public partial class PropertyDefinition
    {
        [DataMember(Name="propertyName")]
        public string PropertyName { get; set; }

        [DataMember(Name="propertyDescription")]
        public string PropertyDescription { get; set; }

        /// <value>
        /// Property <c>DataType</c> represents the type of data with which the property value is expressed. Examples:
        /// "dateTime", "double", "enumeration", "integer", "string".
        /// </value>
        [DataMember(Name="dataType", IsRequired = false)]
        public string DataType { get; set; }

        /// <value>
        /// Property <c>UnitOfMeasure</c> represents the unit of measurement in which the value is expressed.
        /// Examples: "ampere", "cubicMetrePerSecond-Time", "degree", "degreeCelsius", "dimensionless", "hectopascal",
        /// "kilovoltampere", "kilovoltamperereactive", "kilovoltamperereactivePerHour", "kilowatt", "kilowattHour",
        /// "lux", "metrePerSecond-Time", "microgramPerCubicCentimetre", "millimetre", "partsPerMillion",
        /// "squareMetre", "volt", "wattPerSquareMetre".
        /// </value>
        [DataMember(Name="unitOfMeasure", IsRequired = false)]
        public string UnitOfMeasure { get; set; }
        /// <value>
        /// Examples: "average", "instantaneous", "static", "total".
        /// </value>
        [DataMember(Name = "measurementType", IsRequired = false)]
        public string MeasurementType { get; set; }

        [DataMember(Name="subProperties", IsRequired = false)]
        public SubProperties SubProperties { get; set; }

        [DataMember(Name="codeList", IsRequired = false)]
        public Uri CodeList { get; set; }
    }

    [DataContract]
    public partial class SubProperties
    {
        [DataMember(Name="propertyName")]
        public List<string> PropertyName { get; set; }
    }

    [DataContract]
    public partial class Values
    {
        [DataMember(Name="line")]
        public List<Line> Line { get; set; }
    }

    [DataContract]
    public partial class Line
    {
        [DataMember(Name="id")]
        public long Id { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name="coordinates")]
        public Coordinates Coordinates { get; set; }

        [DataMember(Name="period")]
        public Period Period { get; set; }

        [DataMember(Name="property")]
        public List<Property> Property { get; set; }
    }

    [DataContract]
    public partial class Period
    {
        [DataMember(Name="start_ts")]
        public DateTime StartTs { get; set; }

        [DataMember(Name="end_ts")]
        public DateTime EndTs { get; set; }
    }

    [DataContract]
    public partial class Property
    {
        [DataMember(Name="name")]
        public string Name { get; set; }

        [DataMember(Name="val")]
        public string Val { get; set; }

        [DataMember(Name = "property")]
        public List<Property> SubProperties { get; set; }
    }
}
