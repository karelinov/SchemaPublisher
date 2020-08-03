using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EADiagramPublisher.Contracts;
using EADiagramPublisher.Enums;

namespace EADiagramPublisher.Forms
{
    public partial class FSetTags : Form
    {
        private bool wasdblclick = false;


        public FSetTags()
        {
            InitializeComponent();
        }

        public ExecResult<List<TagData>> Execute(List<TagData> tagData)
        {
            ExecResult<List<TagData>> result = new ExecResult<List<TagData>>() { value = new List<TagData>() };
            try
            {
                // Заливаем список Tag в lvTags
                lvTags.Items.Clear();

                foreach (var tagDataItem in tagData)
                {
                    ListViewItem item = new ListViewItem();
                    item.Checked = tagDataItem.TagState;
                    item.SubItems.Add(tagDataItem.TagName);
                    item.SubItems.Add(tagDataItem.TagValue);
                    item.SubItems.Add(tagDataItem.Count.ToString());
                    item.SubItems.Add(tagDataItem.Ex.ToString());
                    item.Tag = tagDataItem;

                    lvTags.Items.Add(item);

                }



                DialogResult res = this.ShowDialog();
                if (res == DialogResult.OK)
                {
                    result.code = 0;

                    // Собираем информацию lvTags в объект результата
                    foreach (ListViewItem item in lvTags.Items)
                    {
                        result.value.Add((TagData)item.Tag);
                    }
                }
                else
                {
                    result.code = (int)DialogResult.Cancel;
                }

            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }

        private void lvTags_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem selectedItem = lvTags.SelectedItems[0];

            /*
            if (((TagData)selectedItem.Tag).Ex == true)
            {
                MessageBox.Show("Нельзя редактировать унаследованные занчения");
            }
            else
            {
            */

            ExecResult<TagData> editDialogResult = new FAddTag().Execute(DAConst.StandardTags, false, (TagData)selectedItem.Tag);

            if (editDialogResult.code == 0)
            {
                selectedItem.SubItems[1].Text = editDialogResult.value.TagValue;
                editDialogResult.value.Ex = false;
                selectedItem.Tag = editDialogResult.value;
            }
            else if (editDialogResult.code == -1)
            {
                throw new Exception(editDialogResult.message);
            }




            wasdblclick = true;

        }

        private void tsbAdd_Click(object sender, EventArgs e)
        {
            ExecResult<TagData> addDialogResult = new FAddTag().Execute(DAConst.StandardTags);

            if (addDialogResult.code == 0)
            {
                foreach (ListViewItem curItem in lvTags.Items)
                {
                    if (((TagData)curItem.Tag).TagName == addDialogResult.value.TagName && ((TagData)curItem.Tag).Ex == false)
                    {
                        MessageBox.Show("такой элемент уже есть");
                        return;
                    }
                }

                ListViewItem item = new ListViewItem();
                item.Checked = true;
                item.SubItems.Add(addDialogResult.value.TagName);
                item.SubItems.Add(addDialogResult.value.TagValue);
                item.SubItems.Add("");
                item.SubItems.Add("");
                item.Tag = addDialogResult.value;

                lvTags.Items.Add(item);


            }

        }

        private void lvTags_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (((TagData)lvTags.Items[e.Index].Tag).Ex == true)
            {
                e.NewValue = e.CurrentValue;
                //MessageBox.Show("Нельзя редактировать унаследованные значения");

            }
            else
            {
                ((TagData)lvTags.Items[e.Index].Tag).TagState = (e.NewValue == CheckState.Checked);
            }
        }

    }


    public class ListViewEx : ListView
    {
        private bool m_ByDoubleClick = false; // indicate if the click is double click

        protected override void OnItemCheck(System.Windows.Forms.ItemCheckEventArgs ice)
        {
            if (m_ByDoubleClick)
            {
                ice.NewValue = ice.CurrentValue;
                m_ByDoubleClick = false;
            }
            else
                base.OnItemCheck(ice);
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Clicks == 2)
                m_ByDoubleClick = true; // Set to true here since Clicks equals 2 (a double click)

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            m_ByDoubleClick = false; // Set to false by default
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            // The following code is to prevent the MouseDoubleClick event if the double click is on the CheckBox
            ListViewHitTestInfo ti = HitTest(e.X, e.Y);
            if (ti.Location != ListViewHitTestLocations.StateImage)
                base.OnMouseDoubleClick(e);
        }
    }
}
