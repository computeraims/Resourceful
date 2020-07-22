using HarmonyLib;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Resourceful
{
    class ResourcefulManager : MonoBehaviour
    {
        public static List<ushort> autoPickupItems = new List<ushort>();

        public void Awake()
        {
            Console.WriteLine("ResourcefulManager loaded");
            autoPickupItems.Add(67); // Metal Scrap
            autoPickupItems.Add(37); // Birch Log
            autoPickupItems.Add(39); // Maple Log
            autoPickupItems.Add(41); // Pine Log
            autoPickupItems.Add(38); // Birch Stick
            autoPickupItems.Add(40); // Maple Stick
            autoPickupItems.Add(42); // Pine Stick
        }

        public static void getItemsInRadius(Vector3 center, float sqrRadius, List<RegionCoordinate> search, List<ItemData> result)
        {
            if (ItemManager.regions == null)
            {
                return;
            }
            for (int i = 0; i < search.Count; i++)
            {
                RegionCoordinate regionCoordinate = search[i];
                if (ItemManager.regions[(int)regionCoordinate.x, (int)regionCoordinate.y] != null)
                {
                    for (int j = 0; j < ItemManager.regions[(int)regionCoordinate.x, (int)regionCoordinate.y].items.Count; j++)
                    {
                        ItemData itemDrop = ItemManager.regions[(int)regionCoordinate.x, (int)regionCoordinate.y].items[j];
                        if ((itemDrop.point - center).sqrMagnitude < sqrRadius)
                        {
                            result.Add(itemDrop);
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(ResourceManager))]
        [HarmonyPatch("damage")]
        class TellResourceDeadPatch
        {
            static void Postfix(Transform resource, Vector3 direction, float damage, float times, float drop, ref EPlayerKill kill, ref uint xp, CSteamID instigatorSteamID, EDamageOrigin damageOrigin, bool trackKill)
            {
                if (kill == EPlayerKill.RESOURCE)
                {
                    Player ply = PlayerTool.getPlayer(instigatorSteamID);


                    List<RegionCoordinate> region = new List<RegionCoordinate>();
                    region.Add(new RegionCoordinate(ply.movement.region_x, ply.movement.region_y));

                    List<ItemData> searchResult = new List<ItemData>();

                    getItemsInRadius(ply.transform.position, 200f, region, searchResult);

                    foreach (ItemData item in searchResult)
                    {
                        ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, item.item.id);

                        byte page;
                        byte x;
                        byte y;
                        byte rot;

                        if (ply.inventory.tryFindSpace(itemAsset.size_x, itemAsset.size_y, out page, out x, out y, out rot))
                        {
                            if (!autoPickupItems.Contains(item.item.id)) {
                                return;
                            }

                            ItemManager.instance.askTakeItem(ply.channel.owner.playerID.steamID, ply.movement.region_x, ply.movement.region_y, item.instanceID, x, y, rot, page);
                        }
                    }
                }
            }
        }
    }
}
