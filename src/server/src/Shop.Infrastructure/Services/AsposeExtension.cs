﻿using Microsoft.AspNetCore.Builder;
using System;
using System.IO;

namespace Shop.Infrastructure.Services;

/// <summary>
/// 设置Aspose License
/// </summary>
public static class AsposeExtension
{
    ///// <summary>
    ///// 设置Aspose License，包括Words和Cells(Excel)组件
    ///// </summary>
    ///// <param name="app"><see cref="IApplicationBuilder"/></param>
    //public static void UseAspose(this IApplicationBuilder app)
    //{
    //    var base64LicenseString = "PExpY2Vuc2U+CiAgPERhdGE+CiAgICA8TGljZW5zZWRUbz5TdXpob3UgQXVuYm94IFNvZnR3YXJlIENvLiwgTHRkLjwvTGljZW5zZWRUbz4KICAgIDxFbWFpbFRvPnNhbGVzQGF1bnRlYy5jb208L0VtYWlsVG8+CiAgICA8TGljZW5zZVR5cGU+RGV2ZWxvcGVyIE9FTTwvTGljZW5zZVR5cGU+CiAgICA8TGljZW5zZU5vdGU+TGltaXRlZCB0byAxIGRldmVsb3BlciwgdW5saW1pdGVkIHBoeXNpY2FsIGxvY2F0aW9uczwvTGljZW5zZU5vdGU+CiAgICA8T3JkZXJJRD4xOTA4MjYwODA3NTM8L09yZGVySUQ+CiAgICA8VXNlcklEPjEzNDk3NjAwNjwvVXNlcklEPgogICAgPE9FTT5UaGlzIGlzIGEgcmVkaXN0cmlidXRhYmxlIGxpY2Vuc2U8L09FTT4KICAgIDxQcm9kdWN0cz4KICAgICAgPFByb2R1Y3Q+QXNwb3NlLlRvdGFsIGZvciAuTkVUPC9Qcm9kdWN0PgogICAgPC9Qcm9kdWN0cz4KICAgIDxFZGl0aW9uVHlwZT5FbnRlcnByaXNlPC9FZGl0aW9uVHlwZT4KICAgIDxTZXJpYWxOdW1iZXI+M2U0NGRlMzAtZmNkMi00MTA2LWIzNWQtNDZjNmEzNzE1ZmMyPC9TZXJpYWxOdW1iZXI+CiAgICA8U3Vic2NyaXB0aW9uRXhwaXJ5PjIwMjAwODI3PC9TdWJzY3JpcHRpb25FeHBpcnk+CiAgICA8TGljZW5zZVZlcnNpb24+My4wPC9MaWNlbnNlVmVyc2lvbj4KICAgIDxMaWNlbnNlSW5zdHJ1Y3Rpb25zPmh0dHBzOi8vcHVyY2hhc2UuYXNwb3NlLmNvbS9wb2xpY2llcy91c2UtbGljZW5zZTwvTGljZW5zZUluc3RydWN0aW9ucz4KICA8L0RhdGE+CiAgPFNpZ25hdHVyZT53UGJtNUt3ZTYvRFZXWFNIY1o4d2FiVEFQQXlSR0pEOGI3L00zVkV4YWZpQnd5U2h3YWtrNGI5N2c2eGtnTjhtbUFGY3J0c0cwd1ZDcnp6MytVYk9iQjRYUndTZWxsTFdXeXNDL0haTDNpN01SMC9jZUFxaVZFOU0rWndOQkR4RnlRbE9uYTFQajhQMzhzR1grQ3ZsemJLZFZPZXk1S3A2dDN5c0dqYWtaL1E9PC9TaWduYXR1cmU+CjwvTGljZW5zZT4=";
    //    new Aspose.Cells.License().SetLicense(new MemoryStream(Convert.FromBase64String(base64LicenseString)));
    //    new Aspose.Words.License().SetLicense(new MemoryStream(Convert.FromBase64String(base64LicenseString)));
    //}
    
    ///// <summary>
    ///// 设置Aspose.Words组件的License
    ///// </summary>
    ///// <param name="app"><see cref="IApplicationBuilder"/></param>
    //public static void UseAsposeWords(this IApplicationBuilder app)
    //{
    //    var base64LicenseString = "PExpY2Vuc2U+CiAgPERhdGE+CiAgICA8TGljZW5zZWRUbz5TdXpob3UgQXVuYm94IFNvZnR3YXJlIENvLiwgTHRkLjwvTGljZW5zZWRUbz4KICAgIDxFbWFpbFRvPnNhbGVzQGF1bnRlYy5jb208L0VtYWlsVG8+CiAgICA8TGljZW5zZVR5cGU+RGV2ZWxvcGVyIE9FTTwvTGljZW5zZVR5cGU+CiAgICA8TGljZW5zZU5vdGU+TGltaXRlZCB0byAxIGRldmVsb3BlciwgdW5saW1pdGVkIHBoeXNpY2FsIGxvY2F0aW9uczwvTGljZW5zZU5vdGU+CiAgICA8T3JkZXJJRD4xOTA4MjYwODA3NTM8L09yZGVySUQ+CiAgICA8VXNlcklEPjEzNDk3NjAwNjwvVXNlcklEPgogICAgPE9FTT5UaGlzIGlzIGEgcmVkaXN0cmlidXRhYmxlIGxpY2Vuc2U8L09FTT4KICAgIDxQcm9kdWN0cz4KICAgICAgPFByb2R1Y3Q+QXNwb3NlLlRvdGFsIGZvciAuTkVUPC9Qcm9kdWN0PgogICAgPC9Qcm9kdWN0cz4KICAgIDxFZGl0aW9uVHlwZT5FbnRlcnByaXNlPC9FZGl0aW9uVHlwZT4KICAgIDxTZXJpYWxOdW1iZXI+M2U0NGRlMzAtZmNkMi00MTA2LWIzNWQtNDZjNmEzNzE1ZmMyPC9TZXJpYWxOdW1iZXI+CiAgICA8U3Vic2NyaXB0aW9uRXhwaXJ5PjIwMjAwODI3PC9TdWJzY3JpcHRpb25FeHBpcnk+CiAgICA8TGljZW5zZVZlcnNpb24+My4wPC9MaWNlbnNlVmVyc2lvbj4KICAgIDxMaWNlbnNlSW5zdHJ1Y3Rpb25zPmh0dHBzOi8vcHVyY2hhc2UuYXNwb3NlLmNvbS9wb2xpY2llcy91c2UtbGljZW5zZTwvTGljZW5zZUluc3RydWN0aW9ucz4KICA8L0RhdGE+CiAgPFNpZ25hdHVyZT53UGJtNUt3ZTYvRFZXWFNIY1o4d2FiVEFQQXlSR0pEOGI3L00zVkV4YWZpQnd5U2h3YWtrNGI5N2c2eGtnTjhtbUFGY3J0c0cwd1ZDcnp6MytVYk9iQjRYUndTZWxsTFdXeXNDL0haTDNpN01SMC9jZUFxaVZFOU0rWndOQkR4RnlRbE9uYTFQajhQMzhzR1grQ3ZsemJLZFZPZXk1S3A2dDN5c0dqYWtaL1E9PC9TaWduYXR1cmU+CjwvTGljZW5zZT4=";
    //    new Aspose.Words.License().SetLicense(new MemoryStream(Convert.FromBase64String(base64LicenseString)));
    //}

    /// <summary>
    /// 设置Aspose.Cells(Excel)组件的License
    /// </summary>
    /// <param name="app"><see cref="IApplicationBuilder"/></param>
    public static void UseAsposeCells(this IApplicationBuilder app)
    {
        var base64LicenseString = "PExpY2Vuc2U+CiAgPERhdGE+CiAgICA8TGljZW5zZWRUbz5TdXpob3UgQXVuYm94IFNvZnR3YXJlIENvLiwgTHRkLjwvTGljZW5zZWRUbz4KICAgIDxFbWFpbFRvPnNhbGVzQGF1bnRlYy5jb208L0VtYWlsVG8+CiAgICA8TGljZW5zZVR5cGU+RGV2ZWxvcGVyIE9FTTwvTGljZW5zZVR5cGU+CiAgICA8TGljZW5zZU5vdGU+TGltaXRlZCB0byAxIGRldmVsb3BlciwgdW5saW1pdGVkIHBoeXNpY2FsIGxvY2F0aW9uczwvTGljZW5zZU5vdGU+CiAgICA8T3JkZXJJRD4xOTA4MjYwODA3NTM8L09yZGVySUQ+CiAgICA8VXNlcklEPjEzNDk3NjAwNjwvVXNlcklEPgogICAgPE9FTT5UaGlzIGlzIGEgcmVkaXN0cmlidXRhYmxlIGxpY2Vuc2U8L09FTT4KICAgIDxQcm9kdWN0cz4KICAgICAgPFByb2R1Y3Q+QXNwb3NlLlRvdGFsIGZvciAuTkVUPC9Qcm9kdWN0PgogICAgPC9Qcm9kdWN0cz4KICAgIDxFZGl0aW9uVHlwZT5FbnRlcnByaXNlPC9FZGl0aW9uVHlwZT4KICAgIDxTZXJpYWxOdW1iZXI+M2U0NGRlMzAtZmNkMi00MTA2LWIzNWQtNDZjNmEzNzE1ZmMyPC9TZXJpYWxOdW1iZXI+CiAgICA8U3Vic2NyaXB0aW9uRXhwaXJ5PjIwMjAwODI3PC9TdWJzY3JpcHRpb25FeHBpcnk+CiAgICA8TGljZW5zZVZlcnNpb24+My4wPC9MaWNlbnNlVmVyc2lvbj4KICAgIDxMaWNlbnNlSW5zdHJ1Y3Rpb25zPmh0dHBzOi8vcHVyY2hhc2UuYXNwb3NlLmNvbS9wb2xpY2llcy91c2UtbGljZW5zZTwvTGljZW5zZUluc3RydWN0aW9ucz4KICA8L0RhdGE+CiAgPFNpZ25hdHVyZT53UGJtNUt3ZTYvRFZXWFNIY1o4d2FiVEFQQXlSR0pEOGI3L00zVkV4YWZpQnd5U2h3YWtrNGI5N2c2eGtnTjhtbUFGY3J0c0cwd1ZDcnp6MytVYk9iQjRYUndTZWxsTFdXeXNDL0haTDNpN01SMC9jZUFxaVZFOU0rWndOQkR4RnlRbE9uYTFQajhQMzhzR1grQ3ZsemJLZFZPZXk1S3A2dDN5c0dqYWtaL1E9PC9TaWduYXR1cmU+CjwvTGljZW5zZT4=";
        new Aspose.Cells.License().SetLicense(new MemoryStream(Convert.FromBase64String(base64LicenseString)));
    }
}