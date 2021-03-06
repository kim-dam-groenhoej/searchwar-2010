﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Searchwar_netModel;

/// <summary>
/// Delete/Insert/Update SiteMapNodeRoles
/// </summary>
namespace SearchWar.SiteMap
{
    public class SiteMapNodeRole : CustomSiteMapSystem
    {
        public SiteMapNodeRole()
        {

            _roleIds = new List<Guid>();

        }

        #region deleteSiteMapRoles
        // list of aspnet_role ids
        private List<Guid> _roleIds;


        /// <summary>
        /// Insert roleId from List for Insert/Delete SiteMapNodeRoles from SiteMapNodes
        /// 
        /// Use AddAllSiteMapNodeRoles or RemoveAllSiteMapNodeRoles
        /// </summary>
        /// <param name="roleId">Insert RoleId</param>
        public void AddRoleIdToList(Guid roleId) {

            _roleIds.Add(roleId);

        }


        /// <summary>
        /// Remove roleId from List for Insert/Delete SiteMapNodeRoles from SiteMapNodes
        /// 
        /// Use AddAllSiteMapNodeRoles or RemoveAllSiteMapNodeRoles
        /// </summary>
        /// <param name="roleId">Insert RoleId</param>
        public void RemoveRoleIdFromList(Guid roleId) {

            _roleIds.Remove(roleId);

        }


        /// <summary>
        /// Clear List of RoleIds
        /// </summary>
        public void ClearListOfRoleIds() {

            _roleIds.Clear();

        }


        /// <summary>
        /// Remove SiteMapNodeRoles from cSiteMapNode 
        /// (Insert RoleIds with AddRoleId) (Remove RoleIds with RemoveRoleId)
        /// 
        /// Needed to know what cSiteMapNode you want to delete SiteMapNodeRoles from!
        /// (Insert SiteMapNodeIds with AddSiteMapNodeIdToList) (Remove SiteMapNodeIds with RemoveSiteMapNodeIdFromList)
        /// </summary>
        public void RemoveAllSiteMapNodeRoles() {
            // using statement free the memory after metode is done
            using (Searchwar_netEntities db = new Searchwar_netEntities()) {
                // Check RemoveRoleIds for empty
                if (_roleIds.Count() != 0) {
                    // Select all SiteMapNodeRoles
                    IQueryable<SW_SiteMapNodeRole> getSiteMapNodeRoles = (from r in db.SW_SiteMapNodeRoles
                                                                          where _roleIds.Contains(r.RoleId)
                                                                          select r).AsQueryable<SW_SiteMapNodeRole>();

                    // Convert to List and Remove all SiteMapNodeRoles
                    foreach (var item in getSiteMapNodeRoles)
                    {
                        db.SW_SiteMapNodeRoles.DeleteObject(item);
                    }
                }

                db.SaveChanges();
            }

        } 
        #endregion


        /// <summary>
        /// Insert SiteMapNodeRoles to cSiteMapNode 
        /// (Insert RoleIds with AddRoleId) (Remove RoleIds with RemoveRoleId)
        /// 
        /// Needed to know what cSiteMapNode you want to Insert SiteMapNodeRoles to!
        /// (Insert SiteMapNodeIdsToList with AddSiteMapNodeId) (Remove SiteMapNodeIds with RemoveSiteMapNodeIdFromList)
        /// </summary>
        /// <param name="userId">Insert UserId</param>
        /// <param name="siteMapNodeId">Insert siteMapNodeId</param>
        public void AddAllSiteMapNodeRoles(Guid userId, int siteMapNodeId)
        {
            DateTime dateTimeNow = TimeZoneManager.DateTimeNow;
            List<SW_SiteMapNodeRole> listOfAddedSiteMapNodeRoles;


            // using statement free the memory after metode is done
            using (Searchwar_netEntities db = new Searchwar_netEntities())
            {

                // Check Added RoleIds for empty
                if (_roleIds.Count() != 0)
                {
                    listOfAddedSiteMapNodeRoles = new List<SW_SiteMapNodeRole>();

                        foreach (Guid roleId in _roleIds)
                        {
                            aspnet_Role getRoleId = (db.aspnet_Roles.Where(r => r.RoleId.Equals(roleId))).SingleOrDefault();

                            // Check roleid in database
                            if (getRoleId != null)
                            {
                                SW_SiteMapNode getCurrentSiteMapNode = (db.SW_SiteMapNode.Where(
                                    s => s.SiteMapNodeId.Equals(siteMapNodeId))).SingleOrDefault();

                                // Check roleid in database
                                if (getCurrentSiteMapNode != null)
                                {

                                    SW_SiteMapNodeRole createRoleToSiteMapNode = new SW_SiteMapNodeRole
                                                                                     {
                                                                                         SiteMapNodeRoleAddedDate =
                                                                                             dateTimeNow,
                                                                                         SiteMapNodeRoleAddedUserId =
                                                                                             userId,
                                                                                         RoleId = roleId,
                                                                                         SiteMapNodeId = siteMapNodeId
                                                                                     };

                                    listOfAddedSiteMapNodeRoles.Add(createRoleToSiteMapNode);

                                }
                                else
                                {

                                    throw (new ArgumentNullException("siteMapNodeId", "Cant find siteMapNodeId '" + siteMapNodeId + "' in database"));

                                }
                            }
                            else
                            {


                                throw (new ArgumentNullException("roleId", "Cant find RoleId '" + roleId + "' in database"));

                            }
                        }

                        foreach (var item in listOfAddedSiteMapNodeRoles)
                        {
                            db.SW_SiteMapNodeRoles.AddObject(item);
                        }

                }

                db.SaveChanges();
            }

        }
    }
}