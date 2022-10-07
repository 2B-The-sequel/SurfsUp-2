using Microsoft.AspNetCore.Mvc;
using SurfsUp.Models;

namespace SurfsUp.Controllers
{
    public abstract class LockableController : Controller
    {
        private readonly static List<Lock> locks = new();
        private readonly static object locksLock = new();

        protected bool Lock(int? id)
        {
            bool found = false;

            lock (locksLock)
            {
                int i = 0;

                while (i < locks.Count && !found)
                {
                    if (locks[i].Id == id)
                    {
                        if ((DateTime.Now - locks[i].Time).TotalSeconds >= 60 * 5)
                            locks.Remove(locks[i]);
                        else
                            found = true;
                    }
                    else
                        i++;
                }

                if (!found)
                    locks.Add(new Lock((int)id, DateTime.Now));
            }

            return found;
        }

        protected void Unlock(int? id)
        {
            lock (locksLock)
            {
                int i = 0;
                bool found = false;
                while (i < locks.Count && !found)
                {
                    if (locks[i].Id == id)
                    {
                        found = true;
                        locks.Remove(locks[i]);
                    }
                    else
                        i++;
                }
            }
        }
    }
}
