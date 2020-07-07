using System;
using System.Collections.Generic;
using System.Text;

namespace OneMFS.SharedResources
{
    public class JwtSettings
    {
        public JwtModel Initiate()
        {
            JwtModel model = new JwtModel();
            model.Key = "MFS@Db(LTD/Am!N@12345)oblOkW@lL3t";
            model.Audience = "MfsAudience";
            model.MinutesToExpiration = 60;
            return model;
        }
    }
}
