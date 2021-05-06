using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using TMFormat.Formats;
using TMFormat.Helpers;

namespace ABOCreatureEditor
{
    public partial class FrmMain : Form
    {
        public int SelectedIndex;
        public string FileName;
        TMCreature Creature = new TMCreature();

        public FrmMain()
        {
            InitializeComponent();
        }

        void FrmMain_Load(object sender, EventArgs e)
        {
            CmbDir.SelectedIndex = 0;

            ContextMenu cm = new ContextMenu();
            cm.MenuItems.Add("Delete");
            cm.MenuItems[0].Click += onLootDelete;
            LstLoots.ContextMenu = cm;
            onCreatureReload();
        }

        void onLootDelete(object sender, EventArgs e)
        {
            if (LstLoots.SelectedIndex >= 0)
            {
                if (SelectedIndex < Creature.loots.Count)
                {
                    Creature.loots.RemoveAt(LstLoots.SelectedIndex);
                    onLoadLoots(Creature.loots);
                }
            }
        }

        void onLoadAnimations(TMCreatureTexture item)
        {
            LstAnimations.Items.Clear();

            string _caption = string.Empty;

            switch (SelectedIndex)
            {
                case 0:
                    _caption = "North_";
                    break;
                case 1:
                    _caption = "East_";
                    break;
                case 2:
                    _caption = "South_";
                    break;
                case 3:
                    _caption = "West_";
                    break;
                case 4:
                    _caption = "Dead_";
                    break;
            }

            for (int i=0; i< item.textures.Count;i++)
            {
               string  _item = _caption + (i+1);
                LstAnimations.Items.Add(_item);
            }

            onCreatureReload();
        }

        void onLoadLoots(List<TMCreatureLoot> items)
        {
            LstLoots.Items.Clear();

            string _caption ="Loot";

            foreach(var item in items)
            {
                string _item = string.Format("{0}_{1}_{2}_{3}", _caption, item.id, item.count, item.probability);
                LstLoots.Items.Add(_item);
            }

            onLootReload();
        }

        void onLootReload()
        {
            
        }

        Bitmap ImageResize(Image image, int width = 32, int height = 32)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        void picTexture1_Click(object sender, EventArgs e)
        {
            PictureBox picture = (sender as PictureBox);

            OpenFileDialog dialog = new OpenFileDialog();

            var codecs = ImageCodecInfo.GetImageEncoders();
            var codecFilter = "Image Files|";
            foreach (var codec in codecs)
            {
                codecFilter += codec.FilenameExtension + ";";
            }
            dialog.Filter = codecFilter;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Image img = Image.FromFile(dialog.FileName);

                    if (img != null)
                    {
                        img = Functions.ToChangeColor((Bitmap)img);
                        picture.BackgroundImage = ImageResize(img);
                        onSaveTexture();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        void picTexture2_Click(object sender, EventArgs e)
        {
            PictureBox picture = (sender as PictureBox);

            OpenFileDialog dialog = new OpenFileDialog();

            var codecs = ImageCodecInfo.GetImageEncoders();
            var codecFilter = "Image Files|";
            foreach (var codec in codecs)
            {
                codecFilter += codec.FilenameExtension + ";";
            }
            dialog.Filter = codecFilter;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Image img = Image.FromFile(dialog.FileName);
                    if (img != null)
                    {
                        img = Functions.ToChangeColor((Bitmap)img);
                        picture.BackgroundImage = ImageResize(img);
                        onSaveTexture();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        void picTexture3_Click(object sender, EventArgs e)
        {
            PictureBox picture = (sender as PictureBox);

            OpenFileDialog dialog = new OpenFileDialog();

            var codecs = ImageCodecInfo.GetImageEncoders();
            var codecFilter = "Image Files|";
            foreach (var codec in codecs)
            {
                codecFilter += codec.FilenameExtension + ";";
            }
            dialog.Filter = codecFilter;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Image img = Image.FromFile(dialog.FileName);
                    if (img != null)
                    {
                        img = Functions.ToChangeColor((Bitmap)img);
                        picture.BackgroundImage = ImageResize(img);
                        onSaveTexture();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        void picTexture4_Click(object sender, EventArgs e)
        {
            PictureBox picture = (sender as PictureBox);

            OpenFileDialog dialog = new OpenFileDialog();

            var codecs = ImageCodecInfo.GetImageEncoders();
            var codecFilter = "Image Files|";
            foreach (var codec in codecs)
            {
                codecFilter += codec.FilenameExtension + ";";
            }
            dialog.Filter = codecFilter;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Image img = Image.FromFile(dialog.FileName);
                    if (img != null)
                    {
                        img = Functions.ToChangeColor((Bitmap)img);
                        picture.BackgroundImage = ImageResize(img);
                        onSaveTexture();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        void picMask1_Click(object sender, EventArgs e)
        {
            PictureBox picture = (sender as PictureBox);

            OpenFileDialog dialog = new OpenFileDialog();

            var codecs = ImageCodecInfo.GetImageEncoders();
            var codecFilter = "Image Files|";
            foreach (var codec in codecs)
            {
                codecFilter += codec.FilenameExtension + ";";
            }
            dialog.Filter = codecFilter;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Image img = Image.FromFile(dialog.FileName);
                    if (img != null)
                    {
                        img = Functions.ToChangeColor((Bitmap)img);
                        picture.BackgroundImage = ImageResize(img);
                        onSaveTexture();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        void picMask2_Click(object sender, EventArgs e)
        {
            PictureBox picture = (sender as PictureBox);

            OpenFileDialog dialog = new OpenFileDialog();

            var codecs = ImageCodecInfo.GetImageEncoders();
            var codecFilter = "Image Files|";
            foreach (var codec in codecs)
            {
                codecFilter += codec.FilenameExtension + ";";
            }
            dialog.Filter = codecFilter;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Image img = Image.FromFile(dialog.FileName);
                    if (img != null)
                    {
                        img = Functions.ToChangeColor((Bitmap)img);
                        picture.BackgroundImage = ImageResize(img);
                        onSaveTexture();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        void picMask3_Click(object sender, EventArgs e)
        {
            PictureBox picture = (sender as PictureBox);

            OpenFileDialog dialog = new OpenFileDialog();

            var codecs = ImageCodecInfo.GetImageEncoders();
            var codecFilter = "Image Files|";
            foreach (var codec in codecs)
            {
                codecFilter += codec.FilenameExtension + ";";
            }
            dialog.Filter = codecFilter;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Image img = Image.FromFile(dialog.FileName);
                    if (img != null)
                    {
                        img = Functions.ToChangeColor((Bitmap)img);
                        picture.BackgroundImage = ImageResize(img);
                        onSaveTexture();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        void picMask4_Click(object sender, EventArgs e)
        {
            PictureBox picture = (sender as PictureBox);

            OpenFileDialog dialog = new OpenFileDialog();

            var codecs = ImageCodecInfo.GetImageEncoders();
            var codecFilter = "Image Files|";
            foreach (var codec in codecs)
            {
                codecFilter += codec.FilenameExtension + ";";
            }
            dialog.Filter = codecFilter;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Image img = Image.FromFile(dialog.FileName);
                    if (img != null)
                    {
                        img = Functions.ToChangeColor((Bitmap)img);
                        picture.BackgroundImage = ImageResize(img);
                        onSaveTexture();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        void CmbDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedIndex = CmbDir.SelectedIndex;
            var item = Creature.dirs[SelectedIndex];
            onLoadAnimations(item);
        }

        void onSaveTexture()
        {
            var creature = Creature.dirs[SelectedIndex];
            
            creature.textures.Add(TMImageHelper.ToBytes(picTexture1.BackgroundImage.ToStream()));
            creature.textures.Add(TMImageHelper.ToBytes(picTexture2.BackgroundImage.ToStream()));
            creature.textures.Add(TMImageHelper.ToBytes(picTexture3.BackgroundImage.ToStream()));
            creature.textures.Add(TMImageHelper.ToBytes(picTexture4.BackgroundImage.ToStream()));

            creature.masks.Add(TMImageHelper.ToBytes(picMask1.BackgroundImage.ToStream()));
            creature.masks.Add(TMImageHelper.ToBytes(picMask2.BackgroundImage.ToStream()));
            creature.masks.Add(TMImageHelper.ToBytes(picMask3.BackgroundImage.ToStream()));
            creature.masks.Add(TMImageHelper.ToBytes(picMask4.BackgroundImage.ToStream()));

            Creature.dirs[LstAnimations.SelectedIndex] = creature;

        }

        void LstAnimations_SelectedIndexChanged(object sender, EventArgs e)
        {
            onCreatureReload();
        }

        void onCreatureReload()
        {
            if (LstAnimations.SelectedIndex < 0)
            {
                LstAnimations.SelectedIndex = 0;
            }

            var item = Creature.dirs[SelectedIndex];
            var anim = item.textures[LstAnimations.SelectedIndex];

            picTexture1.BackgroundImage = anim.ToImage();
            picTexture2.BackgroundImage = anim.ToImage();
            picTexture3.BackgroundImage = anim.ToImage();
            picTexture4.BackgroundImage = anim.ToImage();

            picMask1.BackgroundImage = anim.ToImage();
            picMask2.BackgroundImage = anim.ToImage();
            picMask3.BackgroundImage = anim.ToImage();
            picMask4.BackgroundImage = anim.ToImage();

            checkAgressive.Checked = Creature.is_agressive;
            checkOffset.Checked = Creature.is_offset;
            checkUseSpell.Checked = Creature.use_spell;

            onLoadLoots(Creature.loots);
        }

        void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "aboChar files (*.abochar)|*.abochar";
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FileName = dialog.FileName;
                Creature = TMCreature.Load(FileName);

                if (Creature != null)
                {
                    onCreatureReload();
                    TxtName.Text = Creature.name;
                    TxtSpeed.Text = Creature.speed.ToString();
                    TxtLevel.Text = Creature.level.ToString();
                    TxtExp.Text = Creature.experience.ToString();
                    TxtHeal.Text = Creature.heal.ToString();
                    TxtAttack.Text = Creature.attack.ToString();
                    TxtDefence.Text = Creature.defence.ToString();
                   
                    checkAgressive.Checked = Creature.is_agressive;
                    checkOffset.Checked = Creature.is_offset;
                    checkUseSpell.Checked = Creature.use_spell;

                    Debug.WriteLine("LOADED OK");

                    this.Text = $"ABO Creature Editor - [{FileName}]";
                }
                else
                {
                    MessageBox.Show(this, "Invalid format", "Invalido");
                }
            }
        }

        void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(FileName))
            {
                Creature.name = TxtName.Text;
                Creature.speed = float.Parse(TxtSpeed.Text);
                Creature.level = int.Parse(TxtLevel.Text);
                Creature.experience = int.Parse(TxtExp.Text);
                Creature.is_agressive = checkAgressive.Checked;
                Creature.heal = int.Parse(TxtHeal.Text);
                Creature.attack = int.Parse(TxtAttack.Text);
                Creature.defence = int.Parse(TxtDefence.Text);
                Creature.is_offset = checkOffset.Checked;
                Creature.use_spell = checkUseSpell.Checked;

                bool status = Creature.SaveToFile(FileName);

                if (status)
                {
                    MessageBox.Show(this, "El Archivo fue guargado correctamente.", "Guardado");
                }
            }
            else
            {
                OnSaveFile();
            }
        }

        void OnSaveFile()
        {
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter = "aboChar files (*.abochar)|*.abochar";
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FileName = dialog.FileName;
                bool status = Creature.SaveToFile(FileName);

                if (status)
                {
                    MessageBox.Show(this, "El Archivo fue guargado correctamente.", "Guardado");
                }

            }
        }

        void TxtName_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox = (sender as TextBox);

            Creature.name = textbox.Text;
        }

        void label5_Click(object sender, EventArgs e)
        {

        }

        void TxtSpeed_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox = (sender as TextBox);
            if (!string.IsNullOrEmpty(textbox.Text))
            {
                Creature.speed = float.Parse(textbox.Text);
            }
        }

        void checkAgressive_CheckedChanged(object sender, EventArgs e)
        {
            Creature.is_agressive = checkAgressive.Checked;
        }

        void checkOffset_CheckedChanged(object sender, EventArgs e)
        {
            Creature.is_offset = checkOffset.Checked;
        }

        void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        void BtnLoot_Click(object sender, EventArgs e)
        {
            TMCreatureLoot loot = new TMCreatureLoot();
            loot.id = int.Parse(TxtLootItemID.Text);
            loot.count = int.Parse(TxtLootCount.Text);
            loot.probability = int.Parse(TxtLootProb.Text);

            if (loot.id <= 0)
            {
                MessageBox.Show("Debes establecer un id del item.","Requerido");
                return;
            }

            Creature.loots.Add(loot);

            TxtLootItemID.Text = "0";
            TxtLootCount.Text = "0";
            TxtLootProb.Text = "0";

            onLoadLoots(Creature.loots);
        }
    }
}
