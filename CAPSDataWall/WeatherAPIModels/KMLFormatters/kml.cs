namespace WeatherAPIModels.KMLFormatters
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://earth.google.com/kml/2.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://earth.google.com/kml/2.0", IsNullable = false)]
    public partial class kml
    {

        private kmlDocument documentField;

        /// <remarks/>
        public kmlDocument Document
        {
            get
            {
                return this.documentField;
            }
            set
            {
                this.documentField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://earth.google.com/kml/2.0")]
    public partial class kmlDocument
    {

        private kmlDocumentGroundOverlay groundOverlayField;

        /// <remarks/>
        public kmlDocumentGroundOverlay GroundOverlay
        {
            get
            {
                return this.groundOverlayField;
            }
            set
            {
                this.groundOverlayField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://earth.google.com/kml/2.0")]
    public partial class kmlDocumentGroundOverlay
    {

        private string nameField;

        private kmlDocumentGroundOverlayIcon iconField;

        private byte visibilityField;

        private kmlDocumentGroundOverlayLatLonBox latLonBoxField;

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public kmlDocumentGroundOverlayIcon Icon
        {
            get
            {
                return this.iconField;
            }
            set
            {
                this.iconField = value;
            }
        }

        /// <remarks/>
        public byte visibility
        {
            get
            {
                return this.visibilityField;
            }
            set
            {
                this.visibilityField = value;
            }
        }

        /// <remarks/>
        public kmlDocumentGroundOverlayLatLonBox LatLonBox
        {
            get
            {
                return this.latLonBoxField;
            }
            set
            {
                this.latLonBoxField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://earth.google.com/kml/2.0")]
    public partial class kmlDocumentGroundOverlayIcon
    {

        private string hrefField;

        /// <remarks/>
        public string href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://earth.google.com/kml/2.0")]
    public partial class kmlDocumentGroundOverlayLatLonBox
    {

        private decimal northField;

        private decimal southField;

        private decimal eastField;

        private decimal westField;

        /// <remarks/>
        public decimal north
        {
            get
            {
                return this.northField;
            }
            set
            {
                this.northField = value;
            }
        }

        /// <remarks/>
        public decimal south
        {
            get
            {
                return this.southField;
            }
            set
            {
                this.southField = value;
            }
        }

        /// <remarks/>
        public decimal east
        {
            get
            {
                return this.eastField;
            }
            set
            {
                this.eastField = value;
            }
        }

        /// <remarks/>
        public decimal west
        {
            get
            {
                return this.westField;
            }
            set
            {
                this.westField = value;
            }
        }
    }
}