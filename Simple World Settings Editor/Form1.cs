using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DamienG.Security.Cryptography;
using Simple.World.Settings.Editor.Classes;
using Simple.World.Settings.Editor.Dialogs;
using Simple.World.Settings.Editor.Enums;
using Simple.World.Settings.Editor.Extensions;

namespace Simple.World.Settings.Editor
{
	public partial class Form1 : Form
	{
		private string loadedFile;
		private Dictionary<string, object> _worldData = new Dictionary<String, Object>();
		private RegSettings _reg;
		public Form1()
		{
			this._reg = new RegSettings("SimpleWorldSettingsEditor");
			this.InitializeComponent();
		}

		private void button1_Click(Object sender, EventArgs e)
		{
			string lastPath;
			using (var ofd = new OpenFileDialog())
			{
				ofd.Filter = "World.dat|world.dat|World.dat.bak|World.dat.bak";
				ofd.Multiselect = false;
				if ((lastPath = this._reg.TryGetValueAs<string>("lastPath")) == null)
					ofd.InitialDirectory =
						Path.GetFullPath(Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "..", 
							"LocalLow", "ProjectorGames", "FortressCraft", "Worlds"));
				else ofd.InitialDirectory = Path.GetFullPath(Path.GetDirectoryName(lastPath));
				if (ofd.ShowDialog() != DialogResult.OK)
					return;
				this._reg["lastPath"] = this.loadedFile = ofd.FileName;
				this.ReadFile(this.loadedFile);
			}
		}

		private void button2_Click(Object sender, EventArgs e)
		{
			// Save Dialog Maybe?
			this.WriteFile(this.loadedFile);
			MessageBox.Show($"Successfully saved \"{this.loadedFile}\". Enjoy your modified save!");
		}

		private void ReadFile(string file)
		{
			using (var fs = File.OpenRead(file))
			using (var reader = new BinaryReader(fs))
			{
				this._worldData.Clear();
				this._worldData.Add("mPath", Path.GetDirectoryName(file), true);
				this._worldData.Add("mnVersion", -1, true);

				var fileVersion = reader.ReadInt32();
				if (fileVersion < 0 || fileVersion > 1) // Not 0 or 1
				{
					MessageBox.Show($"Error, WorldData [{file}] has stupid version of {fileVersion}");
					return;
				}
				if (fileVersion != 1) // Not 1
				{
					MessageBox.Show(fileVersion == 0 // It's a zero
						? "Error, world.dat is full of zeros!"
						: "Error, world.dat needs conversion!");
					return;
				}

				this._worldData.Add("mnVersion", reader.ReadInt32(), true);
				this._worldData.Add("mName", reader.ReadString(), true);
				this._worldData.Add("mnWorldSeed", reader.ReadInt32(), true);
				this._worldData.Add("mrGravity", reader.ReadSingle(), true);
				this._worldData.Add("mrMovementSpeed", reader.ReadSingle(), true);
				this._worldData.Add("mrJumpSpeed", reader.ReadSingle(), true);
				this._worldData.Add("mrMaxFallingSpeed", reader.ReadSingle(), true);
				this._worldData.Add("meGameMode", (GameMode) reader.ReadInt32(), true);
				this._worldData.Add("mInjectionSet", reader.ReadString(), true);
				this._worldData.Add("mnResourceLevel", reader.ReadInt32(), true);
				this._worldData.Add("mnPowerLevel", reader.ReadInt32(), true);
				this._worldData.Add("mnConveyorLevel", reader.ReadInt32(), true);
				this._worldData.Add("mnDayLevel", reader.ReadInt32(), true);
				this._worldData.Add("meDeathEffect", (DeathEffect) reader.ReadInt32(), true);
				this._worldData.Add("mSpawnX", reader.ReadInt64(), true);
				this._worldData.Add("mSpawnY", reader.ReadInt64(), true);
				this._worldData.Add("mSpawnZ", reader.ReadInt64(), true);
				this._worldData.Add("mbIntroCompleted", reader.ReadBoolean(), true);
				this._worldData.Add("mnMobLevel", reader.ReadInt32(), true);
				this._worldData.Add("mbTutorialCompleted", reader.ReadBoolean(), true);
				this._worldData.Add("mbRushMode", reader.ReadBoolean(), true);
				this._worldData.Add("mrWorldTimePlayed", reader.ReadSingle(), true);
				this._worldData.Add("mnSharedResearchMode", reader.ReadInt32(), true);
				this._worldData.Add("mrCurrentTimeOfDay", reader.ReadSingle(), true);
				this._worldData.Add("mCPHCoordX", reader.ReadInt64(), true);
				this._worldData.Add("mCPHCoordY", reader.ReadInt64(), true);
				this._worldData.Add("mCPHCoordZ", reader.ReadInt64(), true);
				this._worldData.Add("mbFlatland", reader.ReadBoolean(), true);
				this._worldData.Add("mbCarePackage", reader.ReadBoolean(), true);
			}

			this.listView1.Items.Clear();
			foreach (var kv in this._worldData)
			{
				var lvi = new ListViewItem {Text = kv.Key};
				if (kv.Value is Int64)
					lvi.SubItems.Add(((Int64) kv.Value).ToString("N0"));
				else if (kv.Value is Int32)
					lvi.SubItems.Add(((Int32) kv.Value).ToString("N0"));
				else if (kv.Value is Single)
					lvi.SubItems.Add(((Single) kv.Value).ToString("N7").TrimEnd('0').TrimEnd('.'));
				else 
					lvi.SubItems.Add(kv.Value.ToString());
				lvi.Tag = kv.Value.GetType();
				this.listView1.Items.Add(lvi);
			}

			if ((Int32) this._worldData["mnVersion"] > 4)
			{
				MessageBox.Show(
					"World Version is higher than this tool supports. There may be unsupported values, and can result in a corrupted world!");
			}

		}

		private void WriteFile(string file)
		{
			var hash = new byte[0];
			var crcFile = $"{file}.crc";

			if (Path.GetExtension(file) == ".bak")
				crcFile = Path.ChangeExtension(file, ".crc.bak");

			using (var fs = File.OpenWrite(file))
			using (var writer = new BinaryWriter(fs))
			{ 
				writer.Write(1); // File Format
				if ((Int32) this._worldData["mnVersion"] != -1)
					writer.Write((Int32) this._worldData["mnVersion"]);
				else writer.Write(4);
				writer.Write((string) this._worldData["mName"]);
				writer.Write((Int32) this._worldData["mnWorldSeed"]);
				writer.Write((Single) this._worldData["mrGravity"]);
				writer.Write((Single) this._worldData["mrMovementSpeed"]);
				writer.Write((Single) this._worldData["mrJumpSpeed"]);
				writer.Write((Single) this._worldData["mrMaxFallingSpeed"]);
				writer.Write((Int32) this._worldData["meGameMode"]);
				writer.Write((string) this._worldData["mInjectionSet"]);
				writer.Write((Int32) this._worldData["mnResourceLevel"]);
				writer.Write((Int32) this._worldData["mnPowerLevel"]);
				writer.Write((Int32) this._worldData["mnConveyorLevel"]);
				writer.Write((Int32) this._worldData["mnDayLevel"]);
				writer.Write((Int32) this._worldData["meDeathEffect"]);
				writer.Write((Int64) this._worldData["mSpawnX"]);
				writer.Write((Int64) this._worldData["mSpawnY"]);
				writer.Write((Int64) this._worldData["mSpawnZ"]);
				writer.Write((Boolean) this._worldData["mbIntroCompleted"]);
				writer.Write((Int32) this._worldData["mnMobLevel"]);
				writer.Write((Boolean) this._worldData["mbTutorialCompleted"]);
				writer.Write((Boolean) this._worldData["mbRushMode"]);
				writer.Write((Single) this._worldData["mrWorldTimePlayed"]);
				writer.Write((Int32) this._worldData["mnSharedResearchMode"]);
				writer.Write((Single) this._worldData["mrCurrentTimeOfDay"]);
				writer.Write((Int64) this._worldData["mCPHCoordX"]);
				writer.Write((Int64) this._worldData["mCPHCoordY"]);
				writer.Write((Int64) this._worldData["mCPHCoordZ"]);
				writer.Write((Boolean) this._worldData["mbFlatland"]);
				writer.Write((Boolean) this._worldData["mbCarePackage"]);
			}

			using (var fs = File.OpenRead(file))
			{
				var crc = new Crc32(3988292384u, 4294967295u);
				hash = crc.ComputeHash(fs);
			}

			using (var fs = File.Open(crcFile, FileMode.OpenOrCreate))
			using (var writer = new StreamWriter(fs))
			{
				for (var i = 0; i < hash.Length; i++)
				{
					writer.Write(hash[i].ToString("X2").ToLower());
				}
			}

		}

		private void listView1_DoubleClick(Object sender, EventArgs e)
		{
			var item = this.listView1.SelectedItems[0];
			if (item == null)
				return;

			if (item.Text == "mPath" || item.Text == "mInjectionSet" || item.Text == "mnVersion")
				return;

			var type = (Type) item.Tag;
			if (type == typeof (Boolean))
			{
				var currentBoolean = (Boolean) this._worldData[item.Text];
				item.SubItems[1].Text = (!currentBoolean).ToString();
				this._worldData[item.Text] = !currentBoolean;
			}
			if (type == typeof (Int64))
			{
				var currentInt = (Int64) this._worldData[item.Text];
				using (var nd = new NumericDialog(currentInt, 0))
				{
					if (nd.ShowDialog() == DialogResult.OK)
					{
						item.SubItems[1].Text = Convert.ToInt64(nd.Result).ToString("N0");
						this._worldData[item.Text] = Convert.ToInt64(nd.Result);
					}
				}
			}
			if (type == typeof (Int32))
			{
				var currentInt = (Int32) this._worldData[item.Text];
				using (var nd = new NumericDialog(currentInt, 0))
				{
					if (nd.ShowDialog() == DialogResult.OK)
					{
						item.SubItems[1].Text = Convert.ToInt32(nd.Result).ToString("N0");
						this._worldData[item.Text] = Convert.ToInt32(nd.Result);
					}
				}
			}
			if (type == typeof (Single))
			{
				var currentFloat = (Single) this._worldData[item.Text];
				using (var nd = new NumericDialog(currentFloat, 7))
				{
					if (nd.ShowDialog() == DialogResult.OK)
					{
						item.SubItems[1].Text = Convert.ToSingle(nd.Result).ToString("N7").TrimEnd('0').TrimEnd('.');
						this._worldData[item.Text] = Convert.ToSingle(nd.Result);
					}
				}
			}
			if (type.BaseType == typeof (Enum))
			{
				var currentEnum = (Enum) this._worldData[item.Text];
				using (var ed = new EnumDialog(currentEnum))
				{
					if (ed.ShowDialog() == DialogResult.OK)
					{
						item.SubItems[1].Text = ed.Result.ToString();
						this._worldData[item.Text] = ed.Result;
					}
				}
			}
			if (type == typeof (string))
			{
				var currentStr = (string) this._worldData[item.Text];
				using (var sd = new StringDialog(currentStr))
				{
					if (sd.ShowDialog() == DialogResult.OK)
					{
						item.SubItems[1].Text = sd.Result;
						this._worldData[item.Text] = sd.Result;
					}
				}
			}
		}

		private static string Combine(params string[] paths)
		{
			return paths.Aggregate(Path.Combine);
		}
	
	}
}
