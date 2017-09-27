﻿using System;
using System.Xml.Serialization;

namespace Federation.Protocols.Request.Elements.Xenc
{
    /// <summary>
    /// ItemsChoice for <c>Referencelists</c>
    /// </summary>
    [Serializable]
    [XmlType(Namespace = Saml20Constants.Xenc, IncludeInSchema = false)]
    public enum ReferenceListType
    {
        /// <summary>
        /// DataReference type.
        /// </summary>
        [XmlEnum("DataReference")]
        DataReference,

        /// <summary>
        /// KeyReference type.
        /// </summary>
        [XmlEnum("KeyReference")]
        KeyReference,
    }
}