using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oauth4Web.Model;
using System.Data.SqlClient;
using MiniNet.Utility.DataBase;
using System.Data;

namespace Oauth4Web.DataAccess
{
    public class OAuthAPIDAL
    {
        public static OAuthAPIEntity Load(string appKey, string userName, int siteid)
        {
            SqlDataReader reader = null;

            try
            {
                string sql = " select id,appkey,appsecret,token,tokensecret,site,username,password from oauthapi where appkey=@appkey and username=@username and site=@siteid";

                SqlParameter[] cols = new SqlParameter[3];
                cols[0] = new SqlParameter("@appkey", appKey);
                cols[1] = new SqlParameter("@username", userName);
                cols[2] = new SqlParameter("@siteid", siteid);

                reader = SqlHelper.ExecuteReader(Config.TwitterConnectionString, CommandType.Text, sql, cols);

                OAuthAPIEntity entity = null;

                if (reader.Read())
                {
                    entity = new OAuthAPIEntity();

                    entity.ID = reader.GetInt32(0);
                    entity.AppKey = reader.GetString(1);
                    entity.AppSecret = reader.GetString(2);
                    entity.Token = reader.GetString(3);
                    entity.TokenSecret = reader.GetString(4);
                    entity.Site = reader.GetInt32(5);
                    entity.UserName = reader.GetString(6);
                    entity.Password = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                }

                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader!=null)
                {
                    reader.Close();
                    reader = null;
                }
            }
        }

        public static void Save(OAuthAPIEntity entity)
        {
            try
            {
                string sql = "insert into oauthapi(appkey,appsecret,token,tokensecret,site,username,password,version) values(@appkey,@appsecret,@token,@tokensecret,@siteid,@username,@password,@version)";

                SqlParameter[] cols = new SqlParameter[8];
                cols[0] = new SqlParameter("@appkey", entity.AppKey);
                cols[1] = new SqlParameter("@appsecret", entity.AppSecret);
                cols[2] = new SqlParameter("@token", entity.Token);
                cols[3] = new SqlParameter("@tokensecret", entity.TokenSecret);
                cols[4] = new SqlParameter("@siteid", entity.Site);
                cols[5] = new SqlParameter("@username", entity.UserName);
                cols[6] = new SqlParameter("@password", entity.Password);
                cols[7] = new SqlParameter("@version", entity.Version);

                SqlHelper.ExecuteNonQuery(Config.TwitterConnectionString, CommandType.Text, sql, cols);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
