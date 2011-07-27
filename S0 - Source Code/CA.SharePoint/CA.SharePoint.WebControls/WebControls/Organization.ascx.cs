using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using System.Collections.Generic;

using Microsoft.SharePoint;
using CA.SharePoint.Utilities.Common;

namespace CA.SharePoint.WebControls
{
    public partial class Organization : System.Web.UI.UserControl
    {
        private List<Employee> employees
        {
            set { ViewState["employees"] = value; }
            get {
                if (ViewState["employees"] == null)
                    ViewState["employees"] = new List<Employee>();

                return (List<Employee>)ViewState["employees"]; }
        }

        protected string strDept = null;
        private List<Employee> heads = new List<Employee>();
        private List<Employee> managers = new List<Employee>();
        private List<Employee> others = new List<Employee>();

        protected void Page_Load(object sender, EventArgs e)
        {
            strDept = Page.Request["Dept"];
            if (string.IsNullOrEmpty(strDept))
                return;

            if (!IsPostBack)
            {
                buildTree();
                //ucUserDetails.Visible = false;
            }

            if (trvEmployees.SelectedNode != null)
            {
                selectUser();
            }
        }

        protected void trvEmployees_SelectedNodeChanged(object sender, EventArgs e)
        {
            selectUser();
        }

        private void buildTree()
        {
            trvEmployees.Nodes.Clear();

            string strSPDept = Page.Request["Dept"];
            bool found = false;
            SPList list = SharePointUtil.GetList(SPContext.Current.Site.RootWeb, CAConstants.ListName.Department);
            foreach (SPListItem item in list.Items)
            {
                if (item["DisplayName"] == null)
                    continue;

                string strTempSPDept = item["DisplayName"].ToString().ToLower();
                if (strTempSPDept == strSPDept.ToLower())
                {
                    employees.AddRange(UserProfileUtil.GetEmployeeFromSSPByDept(item["Name"].ToString()));
                    found = true;
                }
            }

            if (!found)
                employees = UserProfileUtil.GetEmployeeFromSSPByDept(strSPDept);
            Dictionary<string, TreeNode> processed = new Dictionary<string,TreeNode>();
            TreeNode tempRoot = new TreeNode();

            foreach (Employee employee in employees)
            {
                if (processed.Keys.Contains(employee.DisplayName))
                    continue;

                Employee tmpEmployee = employee;
                TreeNode managerNode = null;
                while (tmpEmployee != null)
                {
                    if (processed.Keys.Contains(tmpEmployee.DisplayName))
                    {
                        tmpEmployee = null;
                        continue;
                    }
                    else if (string.IsNullOrEmpty(tmpEmployee.Manager) || (tmpEmployee.UserAccount.Equals(tmpEmployee.Manager)))
                    {
                        //TreeNode newNode = new TreeNode(tmpEmployee.DisplayName, tmpEmployee.DisplayName);
                        TreeNode newNode = new TreeNode();
                        newNode.Value = tmpEmployee.DisplayName;
                        newNode.Text = string.Format("<span onclick=\"return SelectUser('{0}','{1}')\">{0}</span>", tmpEmployee.DisplayName, strSPDept);
                        if (managerNode != null)
                        {
                            newNode.ChildNodes.Add(managerNode);
                        }
                        managerNode = null;
                        tempRoot.ChildNodes.Add(newNode);
                        processed.Add(tmpEmployee.DisplayName, newNode);
                        tmpEmployee = null;
                        continue;
                    }

                    //TreeNode node = new TreeNode(tmpEmployee.DisplayName, tmpEmployee.DisplayName);
                    TreeNode node = new TreeNode();
                    node.Value = tmpEmployee.DisplayName;
                    node.Text = string.Format("<span onclick=\"return SelectUser('{0}','{1}')\">{0}</span>", tmpEmployee.DisplayName, strSPDept);
                    if (managerNode != null)
                    {
                        node.ChildNodes.Add(managerNode);
                    }
                    managerNode = node;
                    processed.Add(tmpEmployee.DisplayName, managerNode);

                    tmpEmployee = employees.Find(new Predicate<Employee>(delegate(Employee emp)
                    {
                        return emp.UserAccount == tmpEmployee.Manager;
                    }));

                    if (tmpEmployee == null)
                    {
                        tempRoot.ChildNodes.Add(managerNode);
                    }
                    else if (processed.TryGetValue(tmpEmployee.DisplayName, out node))
                    {
                        node.ChildNodes.Add(managerNode);
                        tmpEmployee = null;
                        continue;
                    }
                }
            }

            trvEmployees.Nodes.Add(SortTree(tempRoot));
        }

        private TreeNode SortTree(TreeNode tree)
        {
            TreeNode root = new TreeNode("All Employees", Page.Request["Dept"] + " Department");

            foreach (TreeNode child in tree.ChildNodes)
            {
                SortNode(child);
            }

            heads.Sort(delegate(Employee emp1, Employee emp2)
            {
                if (emp1.Department.Equals(emp2.Department, StringComparison.CurrentCultureIgnoreCase))
                    return emp1.DisplayName.CompareTo(emp2.DisplayName);
                else
                    return emp1.Department.CompareTo(emp2.Department);
            });

            if ((managers.Count > 0) && (managers[0].Department.Equals("MTM", StringComparison.CurrentCultureIgnoreCase)))
            {
                managers.Sort(delegate(Employee emp1, Employee emp2)
                {
                    return emp1.Title.CompareTo(emp2.Title);
                });
            }
            else
            {
                managers.Sort(delegate(Employee emp1, Employee emp2)
                {
                    if (emp1.Department.Equals(emp2.Department, StringComparison.CurrentCultureIgnoreCase))
                        return emp1.DisplayName.CompareTo(emp2.DisplayName);
                    else
                        return emp1.Department.CompareTo(emp2.Department);
                });
            }

            others.Sort(delegate(Employee emp1, Employee emp2)
            {
                if (emp1.Department.Equals(emp2.Department, StringComparison.CurrentCultureIgnoreCase))
                    return emp1.DisplayName.CompareTo(emp2.DisplayName);
                else if (emp1.Department.Equals("admin", StringComparison.CurrentCultureIgnoreCase))
                    return 1;
                else if (emp2.Department.Equals("admin", StringComparison.CurrentCultureIgnoreCase))
                    return -1;
                else
                    return emp1.Department.CompareTo(emp2.Department);
            });

            foreach (Employee emp in heads)
            {
                //TreeNode head = new TreeNode("<b>" + emp.DisplayName + "</b>", emp.DisplayName);
                TreeNode head = new TreeNode();
                head.Value = emp.DisplayName;
                head.Text = string.Format("<span style=\"font-weight:bold\" onclick=\"return SelectUser('{0}','{1}')\">{0}</span>", emp.DisplayName, Page.Request["Dept"]);
                root.ChildNodes.Add(head);
            }

            foreach (Employee emp in managers)
            {
                //TreeNode manager = new TreeNode("<b>" + emp.DisplayName + "</b>", emp.DisplayName);
                TreeNode manager = new TreeNode();
                manager.Value = emp.DisplayName;
                manager.Text = string.Format("<span style=\"font-weight:bold\" onclick=\"return SelectUser('{0}','{1}')\">{0}</span>", emp.DisplayName, Page.Request["Dept"]);
                root.ChildNodes.Add(manager);
            }

            foreach (Employee emp in others)
            {
                TreeNode other = new TreeNode();
                other.Value = emp.DisplayName;
                other.Text = string.Format("<span onclick=\"return SelectUser('{0}','{1}')\">{0}</span>", emp.DisplayName, Page.Request["Dept"]);
                //root.ChildNodes.Add(new TreeNode(emp.DisplayName, emp.DisplayName));
                root.ChildNodes.Add(other);
            }

            return root;
        }

        private void SortNode(TreeNode node)
        {
            Employee emp = getEmployee(node);
            if (emp.ApproveRight)
            {
                Employee manager = getEmployee(emp.Manager);
                if (manager == null)
                    heads.Insert(0, emp);
                else
                    managers.Add(emp);
            }
            else
            {
                others.Add(emp);
            }

            foreach (TreeNode child in node.ChildNodes)
            {
                SortNode(child);
            }
        }

        private Employee getEmployee(TreeNode node)
        {
            return employees.Find(new Predicate<Employee>(delegate(Employee emp)
            {
                return emp.DisplayName.ToLower() == node.Value.ToLower();
            }));
        }

        private Employee getEmployee(string userAccount)
        {
            return employees.Find(new Predicate<Employee>(delegate(Employee emp)
            {
                return emp.UserAccount.ToLower() == userAccount.ToLower();
            }));
        }
        public void selectUser()
        {
            //ucUserDetails.User = employees.Find(new Predicate<Employee>(delegate(Employee emp)
            //{
            //    return emp.DisplayName == trvEmployees.SelectedNode.Value;
            //}));

            //ucUserDetails.Visible = (ucUserDetails.User != null);
            //ucUserDetails.BindData();
        }
    }
}