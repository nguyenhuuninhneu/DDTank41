﻿using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using Game.Logic.Effects;

namespace Game.Server.GameServerScript.AI.NPC
{
    public class QXBoss70001 : ABrain
    {
        private int m_attackTurn = 0;
        public int currentCount = 0;
        public int Dander = 0;
        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();

            Body.CurrentDamagePlus = 1;
            Body.CurrentShootMinus = 1;
            Body.SetRect(((SimpleBoss)Body).NpcInfo.X, ((SimpleBoss)Body).NpcInfo.Y, ((SimpleBoss)Body).NpcInfo.Width, ((SimpleBoss)Body).NpcInfo.Height);

            if (Body.Direction == -1)
            {
                Body.SetRect(((SimpleBoss)Body).NpcInfo.X, ((SimpleBoss)Body).NpcInfo.Y, ((SimpleBoss)Body).NpcInfo.Width, ((SimpleBoss)Body).NpcInfo.Height);
            }
            else
            {
                Body.SetRect(-((SimpleBoss)Body).NpcInfo.X - ((SimpleBoss)Body).NpcInfo.Width, ((SimpleBoss)Body).NpcInfo.Y, ((SimpleBoss)Body).NpcInfo.Width, ((SimpleBoss)Body).NpcInfo.Height);
            }

        }

        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnStartAttacking()
        {
            Body.Direction = Game.FindlivingbyDir(Body);
            bool result = false;
            int maxdis = 0;
            foreach (Player player in Game.GetAllFightPlayers())
            {
                if (player.IsLiving && player.X > 919)
                {
                    int dis = (int)Body.Distance(player.X, player.Y);
                    if (dis > maxdis)
                    {
                        maxdis = dis;
                    }
                    result = true;
                }
            }

            if (result)
            {
                KillAttack(919, Game.Map.Info.ForegroundWidth + 1000);
                return;
            }

            if (m_attackTurn == 0)
            {
                AttackA();
                m_attackTurn++;
            }           
            else
            {
                AttackB();
                m_attackTurn = 0;
            }
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();           
        }
        private void KillAttack(int fx, int tx)
        {
            Body.CurrentDamagePlus = 1000f;
            Body.PlayMovie("beatA", 1000, 0);
            Body.RangeAttacking(fx, tx, "cry", 3000, null);
        }
        private void AttackA()
        {
            Body.CurrentDamagePlus = 1.5f;
            Body.PlayMovie("beatA", 1000, 0);
            Body.CallFuction(RangeAttacking, 3000);
        }       
        private void AttackB()
        {
            Body.PlayMovie("beatB", 1000, 0);            
            Body.CallFuction(AddEffect, 3000);

        }

        private void AddEffect()
        {
            //Player target = Game.FindRandomPlayer();  
            ((PVEGame)Game).SendGameFocus(Body, 0, 1000);
            List<Player> players = Game.GetAllLivingPlayers();
            foreach (Player player in players)
            {
                int redBod = (player.MaxBlood * 10) / 100;
                player.AddEffect(new ContinueReduceBlood(2, redBod, player), 0);
            }
        }

        private void RangeAttacking()
        {
            Body.RangeAttacking(0, Game.Map.Info.ForegroundWidth + 1, "cry", 0, null);
        }
    }
}