using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using LIC.Library.Minda.Core.Util;

namespace MessageQueueTestHarness.Utils
{
	public static class PTPTCodeResetter
	{
		public static void ResetPinsByAnimalKeys()
		{
			var db = new DbContext("name=AnimalDB2");
			var db2 = new DbContext("name=Animal_test");


			var users = db.Database.SqlQuery<PfxUser>(
				"select tp.pfx_user.ANML_ID_PFX_CD as ANML_ID_PFX_CD, tp.party.pin as pin from tp.party inner join tp.herd on tp.party.crm_id = tp.herd.crm_id inner join tp.pfx_user on tp.pfx_user.party_id = tp.party.id")
				.ToList();

			users.ForEach(u =>
			{
				var ptptCode = u.ANML_ID_PFX_CD;
				var pin = u.pin;

				if (u.pin != null)
					db2.Database.ExecuteSqlCommand(
						string.Format("update core.Herd set core.Herd.pin = '{0}' where core.Herd.ptptCode = '{1}'", AESTwoWayEncryption.Encrypt(pin.Trim()), ptptCode),
						new SqlParameter[] {});
			});

			db.Dispose();
			db2.Dispose();
		}


		public class PfxUser
		{
			public string ANML_ID_PFX_CD { get; set; }
			public string pin { get; set; }
		}
	}
}