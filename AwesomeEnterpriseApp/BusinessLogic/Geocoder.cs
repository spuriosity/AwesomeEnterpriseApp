using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Net;
using System.Web;
using System.Configuration;
using System.IO;
using System.Xml;

namespace AwesomeEnterpriseApp.BusinessLogic
{

    // Followed Tutorial from http://jasonjano.wordpress.com/2010/02/10/address-geocoding/
    public class Geocoder
    {
	    public string Latitude { get; set; }
	    public string Longitude { get; set; }
	    public string Line1 { get; set; }
	    public string Line2 { get; set; }
	    public string City { get; set; }
	    public string State { get; set; }
	    public string Zip { get; set; }
	    public string Country {get; set;} 

	    private LatLngAccurateToTypes _LatLngAccuracy = 0;
	    public LatLngAccurateToTypes LatLngAccuracy
	    {
		    get { return _LatLngAccuracy; }
	    }

	    public override string ToString()
	    {
		    System.Text.StringBuilder sb = new System.Text.StringBuilder();
		    sb.AppendLine(Line1);
		    if (!string.IsNullOrEmpty(Line2)) sb.AppendLine(Line2);
		    sb.Append(City);
		    sb.Append(", ");
		    sb.Append(State);
		    sb.Append(" ");
		    sb.Append(Zip);
		    sb.Append(" ");
		    sb.Append(Country);
		    return sb.ToString();
	    }

	    public Geocoder()
	    {
	    }

	    public void GeoCode()
	    {
		    string mapskey = "REPLACE_WITH_YOUR_GOOGLE_MAPS_KEY";
		    //setup a streamreader for retrieving data from Google.
		    StreamReader sr = null;

		    //Create the url string to send request
		    string url = string.Format("http://maps.google.com/maps/geo?q={0} +{1} +{2} +{3} +{4}&output=xml&oe=utf8&sensor=false&key={5}", this.Line1, this.City + ", ", this.State, this.Zip, this.Country, mapskey);

		    //Create a web request client.
		    WebClient wc = new WebClient();

		    try
		    {
			    //retrieve result and put it in a streamreader
			    sr = new StreamReader(wc.OpenRead(url));
		    }
		    catch (Exception ex)
		    {
			    throw new Exception("An error occured while retrieving GeoCoded results from Google, the error was: " + ex.Message);
		    }

		    try
		    {
			    //now parse the returned data as an xml
			    XmlTextReader xtr = new XmlTextReader(sr);
			    bool coordread = false;
			    bool accread = false;
			    while (xtr.Read())
			    {
				    xtr.MoveToElement();
				    switch (xtr.Name)
				    {
					    case "AddressDetails": //Check for the address details node
						    while (xtr.MoveToNextAttribute())
						    {
							    switch (xtr.Name)
							    {
								    case "Accuracy": //move into the accuracy attrib and....
									    if (!accread)
									    {
										    //get the accuracy, convert it to our enum and save it to the record
										    this._LatLngAccuracy = (LatLngAccurateToTypes)Convert.ToInt32(xtr.Value);
										    accread = true;
									    }
									    break;
							    }
						    }
						    break;
					    case "coordinates": //the coordinates element
						    if (!coordread)
						    {
							    //move into the element value
							    xtr.Read();

							    //split the coords and then..
							    string[] coords = xtr.Value.Split(new char[] { ',' });

							    //save the values
							    Longitude = coords[0];
							    Latitude = coords[1];

							    //finally, once this has been done, we don't want the process to run again on the same file
							    coordread = true;
						    }
						    break;
				    }
			    }
		    }
		    catch (Exception ex)
		    {
			    throw new Exception("An error occured while parsing GeoCoded results from Google, the error was: " + ex.Message);
		    }
	    }

	    public enum LatLngAccurateToTypes : int
	    {
		    Unknown = 0,
		    Country = 1,
		    Region = 2,
		    SubRegion = 3,
		    Town = 4,
		    PostCode = 5,
		    Street = 6,
		    Intersection = 7,
		    Address = 8,
		    Premises = 9
	    }
    }
}
