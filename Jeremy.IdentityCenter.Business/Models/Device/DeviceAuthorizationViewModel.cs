// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Jeremy.IdentityCenter.Business.Models.Consent;
using System.ComponentModel.DataAnnotations;

namespace Jeremy.IdentityCenter.Business.Models.Device
{
    public class DeviceAuthorizationViewModel : ConsentViewModel
    {
        [Display(Name = "用户编码")]
        public string UserCode { get; set; }

        [Display(Name = "确认用户编码")]
        public bool ConfirmUserCode { get; set; }
    }
}