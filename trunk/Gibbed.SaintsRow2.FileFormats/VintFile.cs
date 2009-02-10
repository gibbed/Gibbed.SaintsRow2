using System;
using System.Collections.Generic;
using System.IO;
using Gibbed.SaintsRow2.Helpers;

namespace Gibbed.SaintsRow2.FileFormats
{
	public static class VintPropertyNames
	{
		private static Dictionary<UInt32, string> GenerateLookup()
		{
			Dictionary<UInt32, string> hashes = new Dictionary<uint, string>();
			string[] table = new string[]
			{
				// element
				"render_mode",
				"visible",
				"mask",
				"offset",
				"anchor",
				"tint",
				"alpha",
				"depth",
				"screen_size",
				"screen_nw",
				"screen_se",
				"rotation",
				"scale",
				"auto_offset",
				"unscaled_size",

				// group
				"screen_size",
				"screen_se",
				"screen_nw",
				"offset",

				// animation
				"start_time",
				"is_paused",
				"target_handle",

				// tween
				"target_handle",
				"target_name",
				"target_property",
				"state",
				"start_time",
				"duration",
				"start_value",
				"end_value",
				"loop_mode",
				"algorithm",
				"max_loops",
				"start_event",
				"end_event",
				"per_frame_event",
				"start_value_type",
				"end_value_type",

				// bitmap
				"custom_source_coords",
				"image",
				"image_crc",
				"source_nw",
				"source_se",

				// gradient
				"gradient_nw",
				"gradient_ne",
				"gradient_sw",
				"gradient_se",
				"alpha_nw",
				"alpha_ne",
				"alpha_sw",
				"alpha_se",

				// text
				"font",
				"text_tag",
				"text_tag_crc",
				"text_scale",
				"word_wrap",
				"wrap_width",
				"vert_align",
				"horz_align",
				"force_case",
				"leading",
				"insert_values",
				"screen_size",
				"line_frame_enable",
				"line_frame_w",
				"line_frame_m",
				"line_frame_e",

				// point
				"screen_size",

				// clip
				"clip_size",
				"clip_enabled",

				// bitmap_circle
				"image",
				"screen_size",
				"source_nw",
				"source_se",
				"start_angle",
				"end_angle",
				"num_wedges",

				// sr2_map
				"zoom",
				"map_mode",

				// video
				"vid_id_handle",
			};


			foreach (string item in table)
			{
				hashes[item.KeyCRC32()] = item;
			}

			return hashes;
		}
		private static Dictionary<UInt32, string> Table = GenerateLookup();
		public static string Lookup(UInt32 hash)
		{
			if (Table.ContainsKey(hash))
			{
				return Table[hash];
			}

			if (hash != 4234861048)
			{
				throw new Exception();
			}

			return "(unknown property : 0x" + hash.ToString("X8") + ")";
		}
	}

	public abstract class VintProperty
	{
		public abstract string Tag { get; }
		public abstract void Read(Stream stream, VintFile vint);
	}

	public class VintIntProperty : VintProperty
	{
		public Int32 Value;

		public override string Tag
		{
			get { return "int"; }
		}

		public override void Read(Stream stream, VintFile vint)
		{
			this.Value = stream.ReadS32();
		}

		public override string ToString()
		{
			return this.Value.ToString();
		}
	}

	public class VintUIntProperty : VintProperty
	{
		public UInt32 Value;

		public override string Tag
		{
			get { return "uint"; }
		}

		public override void Read(Stream stream, VintFile vint)
		{
			this.Value = stream.ReadU32();
		}

		public override string ToString()
		{
			return this.Value.ToString();
		}
	}

	public class VintFloatProperty : VintProperty
	{
		public float Value;

		public override string Tag
		{
			get { return "float"; }
		}

		public override void Read(Stream stream, VintFile vint)
		{
			this.Value = stream.ReadF32();
		}

		public override string ToString()
		{
			return this.Value.ToString();
		}
	}

	public class VintStringProperty : VintProperty
	{
		public string Value;

		public override string Tag
		{
			get { return "string"; }
		}

		public override void Read(Stream stream, VintFile vint)
		{
			this.Value = vint.Strings[stream.ReadS32()];
		}

		public override string ToString()
		{
			return this.Value;
		}
	}

	public class VintBoolProperty : VintProperty
	{
		public bool Value;

		public override string Tag
		{
			get { return "bool"; }
		}

		public override void Read(Stream stream, VintFile vint)
		{
			this.Value = stream.ReadBoolean();
		}

		public override string ToString()
		{
			return this.Value.ToString().ToLowerInvariant();
		}
	}

	public class VintColorProperty : VintProperty
	{
		public float R;
		public float G;
		public float B;

		public override string Tag
		{
			get { return "color"; }
		}

		public override void Read(Stream stream, VintFile vint)
		{
			this.R = stream.ReadF32();
			this.G = stream.ReadF32();
			this.B = stream.ReadF32();
		}

		public override string ToString()
		{
			return
				this.R.ToString() + "," +
				this.G.ToString() + "," +
				this.B.ToString();
		}
	}

	public class VintVector2FProperty : VintProperty
	{
		public float X;
		public float Y;

		public override string Tag
		{
			get { return "vector2f"; }
		}

		public override void Read(Stream stream, VintFile vint)
		{
			this.X = stream.ReadF32();
			this.Y = stream.ReadF32();
		}

		public override string ToString()
		{
			return
				this.X.ToString() + "," +
				this.Y.ToString();
		}
	}

	public class VintObject
	{
		private VintProperty GetProperty(Stream stream, byte propertyType)
		{
			VintProperty value;
			switch (propertyType)
			{
				case 1:
				{
					value = new VintIntProperty();
					break;
				}

				case 2:
				{
					value = new VintUIntProperty();
					break;
				}

				case 3:
				{
					value = new VintFloatProperty();
					break;
				}

				case 4:
				{
					value = new VintStringProperty();
					break;
				}

				case 5:
				{
					value = new VintBoolProperty();
					break;
				}

				case 6:
				{
					value = new VintColorProperty();
					break;
				}

				case 7:
				{
					value = new VintVector2FProperty();
					break;
				}

				default:
				{
					throw new Exception();
				}
			}
			return value;
		}

		public string Name;
		public string Type;

		public Dictionary<string, VintProperty> Baseline = new Dictionary<string, VintProperty>();
		public Dictionary<string, Dictionary<string, VintProperty>> Overrides = new Dictionary<string, Dictionary<string, VintProperty>>();
		public List<VintObject> Children = new List<VintObject>();

		public override string ToString()
		{
			if (this.Name == null || this.Name.Length == 0)
			{
				return base.ToString();
			}

			return this.Name;
		}

		public void Read(Stream stream, VintFile vint)
		{
			this.Name = vint.Strings[stream.ReadS32()];
			this.Type = vint.Strings[stream.ReadS32()];
			UInt16 childCount = stream.ReadU16();
			
			byte unk4 = stream.ReadU8();

			UInt32 unk5 = stream.ReadU32();

			this.Overrides.Clear();
			if (stream.ReadU8() > 0)
			{
				string overrideName = vint.Strings[stream.ReadS32()];
				/*UInt32 overrideSize =*/ stream.ReadU32();

				if (this.Overrides.ContainsKey(overrideName))
				{
					throw new Exception("duplicate override name");
				}

				this.Overrides[overrideName] = new Dictionary<string, VintProperty>();

				while (true)
				{
					byte propertyType = stream.ReadU8();
					if (propertyType == 0)
					{
						break;
					}

					UInt32 hash = stream.ReadU32();
					string propertyName = VintPropertyNames.Lookup(hash);
					if (this.Overrides[overrideName].ContainsKey(propertyName))
					{
						throw new Exception("duplicate override property name");
					}
					this.Overrides[overrideName][propertyName] = this.GetProperty(stream, propertyType);
					this.Overrides[overrideName][propertyName].Read(stream, vint);
				}
			}

			this.Baseline.Clear();
			while (true)
			{
				byte propertyType = stream.ReadU8();
				if (propertyType == 0)
				{
					break;
				}

				UInt32 hash = stream.ReadU32();
				string propertyName = VintPropertyNames.Lookup(hash);
				if (this.Baseline.ContainsKey(propertyName))
				{
					throw new Exception("duplicate baseline property name");
				}
				this.Baseline[propertyName] = this.GetProperty(stream, propertyType);
				this.Baseline[propertyName].Read(stream, vint);
			}

			for (int i = 0; i < childCount; i++)
			{
				VintObject child = new VintObject();
				child.Read(stream, vint);
				this.Children.Add(child);
			}
		}
	}

	public class VintFile
	{
		public UInt16 DocumentType;
		public float AnimationTime;
		public string Name;

		public List<string> CriticalResources = new List<string>();
		public Dictionary<string, string> Metadata = new Dictionary<string, string>();

		public List<string> Strings = new List<string>();

		public List<VintObject> Elements = new List<VintObject>();
		public List<VintObject> Animations = new List<VintObject>();

		private void ReadStrings(Stream stream, UInt32 offset)
		{
			long position = stream.Position;
			stream.Seek(offset, SeekOrigin.Begin);

			UInt32 count = stream.ReadU32();
			UInt32 bufferSize = stream.ReadU32();

			byte[] indexBuffer = new byte[count * 4];
			byte[] stringBuffer = new byte[bufferSize];

			stream.Read(indexBuffer, 0, indexBuffer.Length);
			stream.Read(stringBuffer, 0, stringBuffer.Length);

			stream.Seek(position, SeekOrigin.Begin);

			this.Strings.Clear();
			for (UInt32 i = 0; i < count; i++)
			{
				UInt32 stringOffset = BitConverter.ToUInt32(indexBuffer, (int)(i * 4));
				this.Strings.Add(stringBuffer.GetASCIIZ(stringOffset));
			}
		}

		public void Read(Stream stream)
		{
			if (stream.ReadU32() != 0x3027)
			{
				throw new NotAVintFileException();
			}

			int nameIndex = stream.ReadS32();

			this.DocumentType = stream.ReadU16();
			if (this.DocumentType != 1)
			{
				throw new UnsupportedVintFileVersionException();
			}

			this.AnimationTime = stream.ReadF32();
			UInt32 metadataCount = stream.ReadU32();
			UInt32 criticalResourceCount = stream.ReadU32();
			this.ReadStrings(stream, stream.ReadU32());
			UInt16 elementCount = stream.ReadU16();
			UInt16 animationCount = stream.ReadU16();

			// not absolutely sure this is the name index... so...
			if (nameIndex != 0)
			{
				throw new Exception();
			}

			this.Name = this.Strings[nameIndex];

			for (int i = 0; i < criticalResourceCount; i++)
			{
				byte unk1 = stream.ReadU8();
				int criticalResourceIndex = stream.ReadS32();
				if (unk1 == 0)
				{
					this.CriticalResources.Add(this.Strings[criticalResourceIndex]);
				}
			}

			this.Metadata.Clear();
			for (int i = 0; i < metadataCount; i++)
			{
				int key = stream.ReadS32();
				int value = stream.ReadS32();
				this.Metadata[this.Strings[key]] = this.Strings[value];
			}

			for (int i = 0; i < elementCount; i++)
			{
				VintObject element = new VintObject();
				element.Read(stream, this);
				this.Elements.Add(element);
			}

			for (int i = 0; i < animationCount; i++)
			{
				VintObject animation = new VintObject();
				animation.Read(stream, this);
				this.Animations.Add(animation);
			}
		}
	}
}
