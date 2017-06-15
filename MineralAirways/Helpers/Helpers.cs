using MineralAirways.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Web.Mvc;

namespace MineralAirways.Helpers
{
    public static class ImageActionLinkExtension
    {
        public static MvcHtmlString ImageActionLink(
            this HtmlHelper helper,
            string imageUrl,
            string altText,
            string actionName,
            string controllerName,
            object routeValues,
            object linkHtmlAttributes,
            object imgHtmlAttributes)
        {
            var linkAttributes = AnonymousObjectToKeyValue(linkHtmlAttributes);
            var imgAttributes = AnonymousObjectToKeyValue(imgHtmlAttributes);
            var imgBuilder = new TagBuilder("img");
            imgBuilder.MergeAttribute("src", imageUrl);
            imgBuilder.MergeAttribute("alt", altText);
            imgBuilder.MergeAttributes(imgAttributes, true);
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);
            var linkBuilder = new TagBuilder("a");
            linkBuilder.MergeAttribute("href", urlHelper.Action(actionName, controllerName, routeValues));
            linkBuilder.MergeAttributes(linkAttributes, true);
            var text = linkBuilder.ToString(TagRenderMode.StartTag);
            text += imgBuilder.ToString(TagRenderMode.SelfClosing);
            text += linkBuilder.ToString(TagRenderMode.EndTag);
            return MvcHtmlString.Create(text);
        }

        public static MvcHtmlString ImageActionLink(
            this HtmlHelper helper,
            string imageUrl,
            string altText,
            string actionName,
            object routeValues,
            object imgHtmlAttributes)
        {
            var imgAttributes = AnonymousObjectToKeyValue(imgHtmlAttributes);
            var imgBuilder = new TagBuilder("img");
            imgBuilder.MergeAttribute("src", imageUrl);
            imgBuilder.MergeAttribute("alt", altText);
            imgBuilder.MergeAttributes(imgAttributes, true);
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);
            var linkBuilder = new TagBuilder("a");
            linkBuilder.MergeAttribute("href", urlHelper.Action(actionName, routeValues));
            var text = linkBuilder.ToString(TagRenderMode.StartTag);
            text += imgBuilder.ToString(TagRenderMode.SelfClosing);
            text += linkBuilder.ToString(TagRenderMode.EndTag);
            return MvcHtmlString.Create(text);
        }

        public static MvcHtmlString ImageActionLink(
            this HtmlHelper helper,
            string imageUrl,
            string altText,
            string actionName,
            object routeValues)
        {
            var imgBuilder = new TagBuilder("img");
            imgBuilder.MergeAttribute("src", imageUrl);
            imgBuilder.MergeAttribute("alt", altText);
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);
            var linkBuilder = new TagBuilder("a");
            linkBuilder.MergeAttribute("href", urlHelper.Action(actionName, routeValues));
            var text = linkBuilder.ToString(TagRenderMode.StartTag);
            text += imgBuilder.ToString(TagRenderMode.SelfClosing);
            text += linkBuilder.ToString(TagRenderMode.EndTag);
            return MvcHtmlString.Create(text);
        }

        public static MvcHtmlString ImageActionLink(
            this HtmlHelper helper,
            string imageUrl,
            string altText,
            string actionName)
        {
            var imgBuilder = new TagBuilder("img");
            imgBuilder.MergeAttribute("src", imageUrl);
            imgBuilder.MergeAttribute("alt", altText);
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);
            var linkBuilder = new TagBuilder("a");
            linkBuilder.MergeAttribute("href", urlHelper.Action(actionName));
            var text = linkBuilder.ToString(TagRenderMode.StartTag);
            text += imgBuilder.ToString(TagRenderMode.SelfClosing);
            text += linkBuilder.ToString(TagRenderMode.EndTag);
            return MvcHtmlString.Create(text);
        }

        private static Dictionary<string, object> AnonymousObjectToKeyValue(object anonymousObject)
        {
            var dictionary = new Dictionary<string, object>();
            if (anonymousObject != null)
            {
                foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(anonymousObject))
                {
                    dictionary.Add(propertyDescriptor.Name, propertyDescriptor.GetValue(anonymousObject));
                }
            }
            return dictionary;
        }        
    }

    public static class ImageQr {
        public static string ObtenerImagenQr(ReservasVuelos reserva, string rutaTemp, string rutaTempAux)
        {
            var numVuelo = reserva.Vuelos.NumeroVuelo;
            var numAsiento = reserva.Asientos.Descripcion.Trim();            
            var rutPasajero = reserva.Pasajeros.Rut;

            var nombreArchivo = string.Format("Img_{0}_{1}_{2}.jpg", rutPasajero, numVuelo, numAsiento);
            var fileName = Path.Combine(rutaTemp, nombreArchivo);

            try
            {
                byte[] imgData = File.ReadAllBytes(fileName);
                var srcImgQr = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(imgData));
                return srcImgQr;
            }
            catch (Exception)
            {
                nombreArchivo = @"CodigoQR.png";
                fileName = Path.Combine(rutaTempAux, nombreArchivo);

                byte[] imgData = File.ReadAllBytes(fileName);
                var srcImgQr = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(imgData));
                return srcImgQr;
            }
        }

        public static string ObtenerRutaImagenQr(ReservasVuelos reserva, string rutaTemp, string rutaTempAux) {
            var numVuelo = reserva.Vuelos.NumeroVuelo;
            var numAsiento = reserva.Asientos.Descripcion.Trim();
            var rutPasajero = reserva.Pasajeros.Rut;

            var nombreArchivo = string.Format("Img_{0}_{1}_{2}.jpg", rutPasajero, numVuelo, numAsiento);
            var fileName = Path.Combine(rutaTemp, nombreArchivo);

            try
            {
                byte[] imgData = File.ReadAllBytes(fileName);                
                return fileName;
            }
            catch (Exception)
            {
                nombreArchivo = @"CodigoQR.png";
                fileName = Path.Combine(rutaTempAux, nombreArchivo);                
                return fileName;
            }
        }        
    }   
}