using System;
using System.IO;
using System.Collections.Generic;


namespace CashMachine
{
    public class CashMachine
    {
        int MAX_CREATE_ATTEMPTS = 10;
        string PRODUCT_FILE_LIST_PATH = "C:\\Users\\Tony\\Documents\\Tech Test - Vertical Leap\\ProductList.txt";
        List<string> ExistingProductList = new List<string>();

        public CashMachine()
        {

        }

        public void LoadExistingProducts(/*List<string> products*/)
        {
            StreamReader sr = null;

            try
            {
                // Load the list of current products stored
                sr = new StreamReader(PRODUCT_FILE_LIST_PATH);
            }
            catch (FileNotFoundException fnf)
            {
                Console.WriteLine("Product list file not found, trying to create one.\n");

                int attempts = 0;
                FileStream fs = null;
                try
                {
                    while ((fs == null) && (attempts < MAX_CREATE_ATTEMPTS))
                    {
                        fs = File.Create(PRODUCT_FILE_LIST_PATH);
                        attempts++;
                    }

                    if ((fs == null) || (attempts > MAX_CREATE_ATTEMPTS))
                    {
                        Console.WriteLine("Unable to create product file list, ending process\n");

                    }
                    else if (fs != null)
                    {
                        sr = new StreamReader(PRODUCT_FILE_LIST_PATH);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cannot create product list file\n");
                }
            }
            catch (FieldAccessException fae)
            {
                Console.WriteLine("File Accessing Exception accessing product list file\n");
            }
            catch (Exception e)
            {
                Console.WriteLine("General Exception accessing product list file\n");
            }


            if (sr != null)
            {
                int n = 0;
                while (!sr.EndOfStream)
                {
                    string streamString = sr.ReadLine();

                    if (streamString.Length > 0)
                    {
                        // look for product entries in the 
                        if (streamString.StartsWith('['))
                        {
                            ExistingProductList.Add(streamString);
                        }
                    }
                }
            }
        }

        public string GetShoppingListTotal(List<string> shoppingList)
        {
            //string finalTotal = string.Empty;
            int itemPrice = 0;
            int totalPrice = 0;

            foreach (string item in shoppingList)
            {
                itemPrice = GetItemPrice(item);

                totalPrice += itemPrice;
            }

            return FormatTotalPrice(totalPrice);

        }

        private int GetItemPrice(string item)
        {
            int itemPrice = 0;

            foreach(string product in ExistingProductList)
            {
                if(product.Contains(item))
                {
                    product.Trim();
                    int price = product.IndexOf(',');
                    string strPrice = product.Substring(++price);
                    strPrice = strPrice.Substring(0, strPrice.Length - 1);
                    itemPrice = Convert.ToInt32(strPrice);
                }
            }

            return itemPrice;
        }

        private string FormatTotalPrice(int totalPrice)
        {
            string finalPrice = totalPrice.ToString();

            if (totalPrice > 99)
            {
                finalPrice = finalPrice.Insert(finalPrice.Length - 2, ".");
                finalPrice = string.Format("{0}{1}", "£", finalPrice);
            }
            else
            {
                finalPrice = string.Format("{0}.{1}p", "£", totalPrice);
            }
            
            return finalPrice;
        }
    }
}
