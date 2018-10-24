using System;
using System.Collections.Generic;
using Smobiler.Core.Controls;
using SMOWMS.Domain.Entity;
using SMOWMS.DTOs.InputDTO;
using SMOWMS.CommLib;

namespace SMOWMS.UI.ConsumablesManager
{
    partial class frmConInventoryCreate : Smobiler.Core.Controls.MobileForm
    {
        #region  �������
        private AutofacConfig _autofacConfig = new AutofacConfig();//����������

        private string UserId;
        #endregion
        /// <summary>
        /// ҳ���ʼ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmConInventoryCreate_Load(object sender, EventArgs e)
        {
            try
            {
                UserId = Client.Session["UserID"].ToString();
                if (Client.Session["Role"].ToString() == "SMOWMSAdmin")
                {
                    var user = _autofacConfig.coreUserService.GetUserByID(UserId);
                    String LocationId = user.USER_LOCATIONID;
                    var location = _autofacConfig.wareHouseService.GetByWareID(LocationId);
                    btnWareHouse.Text = location.NAME;
                    btnWareHouse.Tag = LocationId;
                    btnWareHouse.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Toast(ex.Message);
            }
        }
        /// <summary>
        /// �ֿ�ѡ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLocation_Press(object sender, EventArgs e)
        {
            try
            {
                //�ֿ��б���ֵ
                popWareHouse.Groups.Clear();       //�������
                PopListGroup poli = new PopListGroup();
                popWareHouse.Groups.Add(poli);
                List<WareHouse> WHS = _autofacConfig.wareHouseService.GetAllWareHouse();
                foreach (WareHouse Row in WHS)
                {
                    poli.AddListItem(Row.NAME, Row.WAREID);
                }
                if (btnWareHouse.Tag != null)   //�������ѡ�������ʾѡ��Ч��
                {
                    foreach (PopListItem Item in popWareHouse.Groups[0].Items)
                    {
                        if (Item.Value == btnWareHouse.Tag.ToString())
                            popWareHouse.SetSelections(Item);
                    }
                }
                popWareHouse.ShowDialog();
            }
            catch (Exception ex)
            {
                Toast(ex.Message);
            }
        }
        /// <summary>
        /// �洢����ѡ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnST_Press(object sender, EventArgs e)
        {
            try
            {
                if (btnWareHouse.Tag == null) throw new Exception("����ѡ���̵�ֿ�!");
                //�洢���͸�ֵ
                popST.Groups.Clear();       //�������
                PopListGroup poliST = new PopListGroup();
                popST.Groups.Add(poliST);
                List<WHStorageType> WHST = _autofacConfig.wareHouseService.GetSTByWareID(btnWareHouse.Tag.ToString());
                poliST.AddListItem("ȫ��", "");
                foreach (WHStorageType Row in WHST)
                {
                    poliST.AddListItem(Row.NAME, Row.STID);
                }
                if(btnST.Tag != null)
                {
                    foreach (PopListItem Item in popST.Groups[0].Items)
                    {
                        if (Item.Value == btnST.Tag.ToString())
                            popST.SetSelections(Item);
                    }
                }              
                popST.ShowDialog();
            }
            catch (Exception ex)
            {
                Toast(ex.Message);
            }
        }
        /// <summary>
        /// ��λѡ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSL_Press(object sender, EventArgs e)
        {
            try
            {
                if (btnWareHouse.Tag == null) throw new Exception("����ѡ���̵�ֿ�!");
                if (btnST.Tag == null) throw new Exception("����ѡ��洢����!");
                //��λ��ֵ
                popSL.Groups.Clear();       //�������
                PopListGroup poliSL = new PopListGroup();
                popSL.Groups.Add(poliSL);
                poliSL.AddListItem("ȫ��", "");
                List<WHStorageLocation> WHSL = _autofacConfig.wareHouseService.GetSLByWareIDAndSTID(btnWareHouse.Tag.ToString(), btnST.Tag.ToString());
                foreach (WHStorageLocation Row in WHSL)
                {
                    poliSL.AddListItem(Row.NAME, Row.SLID);
                }
                if (btnSL.Tag != null)
                {
                    foreach (PopListItem Item in popSL.Groups[0].Items)
                    {
                        if (Item.Value == btnSL.Tag.ToString())
                            popSL.SetSelections(Item);
                    }
                }
                popSL.ShowDialog();
            }
            catch (Exception ex)
            {
                Toast(ex.Message);
            }
        }
        /// <summary>
        /// �̵���ѡ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnManager_Press(object sender, EventArgs e)
        {
            try
            {
                popMan.Groups.Clear();
                PopListGroup manGroup = new PopListGroup { Title = "�̵���ѡ��" };
                List<coreUser> users = _autofacConfig.coreUserService.GetAll();
                foreach (coreUser Row in users)
                {
                    manGroup.AddListItem(Row.USER_NAME, Row.USER_ID);
                }
                popMan.Groups.Add(manGroup);
                if (btnManager.Tag != null)   //�������ѡ�������ʾѡ��Ч��
                {
                    foreach (PopListItem Item in manGroup.Items)
                    {
                        if (Item.Value == btnManager.Tag.ToString())
                            popMan.SetSelections(Item);
                    }
                }
                popMan.ShowDialog();
            }
            catch (Exception ex)
            {
                Toast(ex.Message);
            }
        }
        /// <summary>
        /// ѡ���̵���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popMan_Selected(object sender, EventArgs e)
        {
            if (popMan.Selection != null)
            {
                btnManager.Text = popMan.Selection.Text + "   > ";
                btnManager.Tag = popMan.Selection.Value;
            }
        }
        /// <summary>
        /// �ֿ�ѡ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popWareHouse_Selected(object sender, EventArgs e)
        {
            if (popWareHouse.Selection != null)
            {
                if (btnWareHouse.Tag != null && popWareHouse.Selection.Value != btnWareHouse.Tag.ToString())
                {
                    MessageBox.Show("�����̵�ֿ⽫�������ѡ�洢���ͺͿ�λ,�Ƿ������", "ϵͳ��ʾ", MessageBoxButtons.YesNo, (object sender1, MessageBoxHandlerArgs args) =>
                    {
                        try
                        {
                            if (args.Result == ShowResult.Yes)
                            {
                                btnWareHouse.Text = popWareHouse.Selection.Text + "   > ";
                                btnWareHouse.Tag = popWareHouse.Selection.Value;
                                //��մ洢���ͺͿ�λ
                                btnST.Text = "ѡ��ѡ�   > ";
                                btnST.Tag = null;
                                btnSL.Text = "ѡ��ѡ�   > ";
                                btnSL.Tag = null;
                            }
                        }
                        catch (Exception ex)
                        {
                            Toast(ex.Message);
                        }
                    });
                }
                else
                {
                    btnWareHouse.Text = popWareHouse.Selection.Text + "   > ";
                    btnWareHouse.Tag = popWareHouse.Selection.Value;
                }
            }
        }
        /// <summary>
        /// ѡ���˴洢����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popST_Selected(object sender, EventArgs e)
        {
            if (popST.Selection != null)
            {
                if (btnST.Tag != null && popST.Selection.Value != btnST.Tag.ToString())
                {
                    MessageBox.Show("�����洢���ͽ��������ѡ��λ,�Ƿ������", "ϵͳ��ʾ", MessageBoxButtons.YesNo, (object sender1, MessageBoxHandlerArgs args) =>
                    {
                        if (args.Result == ShowResult.Yes)
                        {
                            btnST.Text = popST.Selection.Text + "   > ";
                            btnST.Tag = popST.Selection.Value;
                            //��տ�λ
                            btnSL.Text = "ѡ��ѡ�   > ";
                            btnSL.Tag = null;
                        }
                    });
                }
                else
                {
                    btnST.Text = popST.Selection.Text + "   > ";
                    btnST.Tag = popST.Selection.Value;
                }
            }
        }
        /// <summary>
        /// ѡ���˿�λ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popSL_Selected(object sender, EventArgs e)
        {
            if (popSL.Selection != null)
            {
                btnSL.Text = popSL.Selection.Text + "   > ";
                btnSL.Tag = popSL.Selection.Value;
            }
        }
        /// <summary>
        /// �رյ�ǰҳ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void plBack_Press(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// �̵㵥����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void plSave_Press(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtName.Text)) throw new Exception("�̵㵥���Ʋ���Ϊ��!");
                if (btnManager.Tag == null) throw new Exception("�̵��˲���Ϊ��!");
                if (btnWareHouse.Tag == null) throw new Exception("�ֿⲻ��Ϊ��!");

                ConInventoryInputDto conInventoryInput = new ConInventoryInputDto()
                {
                    HANDLEMAN = btnManager.Tag.ToString(),
                    CREATEUSER = UserId,
                    WAREID = btnWareHouse.Tag.ToString(),
                    IsEnd = false,
                    MODIFYUSER = UserId,
                    NAME = txtName.Text
                };
                if (btnST.Tag != null)
                    conInventoryInput.STID = btnST.Tag.ToString();
                if (btnSL.Tag != null)
                    conInventoryInput.SLID = btnSL.Tag.ToString();

                ReturnInfo returnInfo = _autofacConfig.ConInventoryService.AddConInventory(conInventoryInput);
                if (returnInfo.IsSuccess)
                {
                    ShowResult = ShowResult.Yes;
                    Toast("���ӳɹ�");
                    Close();
                }
                else
                {
                    throw new Exception(returnInfo.ErrorInfo);
                }
            }
            catch (Exception ex)
            {
                Toast(ex.Message);
            }
        }
    }
}