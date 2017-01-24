using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.VisualBasic;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private int[] removeFromArray(int[] array, int index) {
            var ints = new List<int>(array);
            ints.RemoveAt(index);
            return ints.ToArray();
        }

        private int[] deleteNumFromArray(int[] array, int num) {
            int x = 0;
            foreach (var i in array) {
                if (i == num) {
                    array = removeFromArray(array, x);
                }

                x++;
            }

            return array;
        }

        private int[] solveSudoku(int[] inSudoku) {

            bool[] solvedNums = new bool[81];
            int[][] nums = new int[81][];
            int solved = 0;

            int x = 0;
            int offset;
            int[] thenums;

            foreach (var a in inSudoku) {
                if (a != 0)
                {
                    solvedNums[x] = true;
                    nums[x] = new int[] {a};
                    solved++;
                } else {
                    solvedNums[x] = false;
                    nums[x] = new int[] {1,2,3,4,5,6,7,8,9};
                }

                x++;
            }

            int i = 0;
            bool changed = false;
            bool broken = false;
            x = 0;
            DateTime start = DateTime.Now;

            int[] square = new int[9] { 0, 1, 2, 9, 10, 11, 18, 19, 20 };
            int[] threes = new int[] { 0, 0, 0, 3, 3, 3, 6, 6, 6, 0, 0, 0, 3, 3, 3, 6, 6, 6, 0, 0, 0, 3, 3, 3, 6, 6, 6, 27, 27, 27, 30, 30, 30, 33, 33, 33, 27, 27, 27, 30, 30, 30, 33, 33, 33, 27, 27, 27, 30, 30, 30, 33, 33, 33, 54, 54, 54, 57, 57, 57, 60, 60, 60, 54, 54, 54, 57, 57, 57, 60, 60, 60, 54, 54, 54, 57, 57, 57, 60, 60, 60 };

            while (solved < 81) {
               // if (x%10 == 0)
               // {
                label1.Text = Math.Round((DateTime.Now - start).TotalSeconds, 2).ToString();
                label1.Refresh();
                //  }
                progressBar1.Value = Convert.ToInt32(Math.Ceiling((solved / 81.0f) * 100.0f));


                if (solvedNums[i] != true) {
                    changed = true;
                    broken = false;
                    foreach (var y in nums[i]) {
                        for (int a = 9; a < 81; a += 9) {
                            if ( solvedNums[(i+a)%81] == true) {
                                if ( nums[(i + a) % 81][0] == y ) {
                                    nums[i] = deleteNumFromArray(nums[i], nums[(i + a) % 81][0]);
                                    if (nums[i].Length == 1) {
                                        solvedNums[i] = true;
                                        solved++;
                                        broken = true;
                                        break;
                                    }
                                    a = 81;
                                }
                            }
                        }
                        if (broken)
                        {
                            break;
                        }
                        offset = Convert.ToInt32(Math.Floor((Convert.ToDouble(i) / 9.0f))) * 9;
                        for (int a = 0; a < 9; a++)
                        {
                            if (a == i - offset) {
                                a++;
                                if (a == 9) {
                                    break;
                                }
                            }
                            if (solvedNums[a + offset] == true)
                            {
                                if (nums[a + offset][0] == y)
                                {
                                    int hij = a + offset;
                                    nums[i] = deleteNumFromArray(nums[i], nums[a+offset][0]);
                                    if (nums[i].Length == 1)
                                    {
                                        solvedNums[i] = true;
                                        solved++;
                                        broken = true;
                                        break;
                                    }
                                    a = 9;
                                }
                            }
                        }
                        if (broken)
                        {
                            break;
                        }
                        offset = threes[i];
                        for (int a = 0; a < 9; a++)
                        {
                            if (square[a] + offset == i)
                            {
                                a++;
                                if (a == 9)
                                {
                                    break;
                                }
                            }
                            if (solvedNums[square[a] + offset] == true)
                            {
                                if (nums[square[a] + offset][0] == y)
                                {
                                    int hij = a + offset;
                                    nums[i] = deleteNumFromArray(nums[i], nums[square[a] + offset][0]);
                                    if (nums[i].Length == 1)
                                    {
                                        solvedNums[i] = true;
                                        solved++;
                                        broken = true;
                                        break;
                                    }
                                    a = 9;
                                }
                            }
                        }
                        if (broken) {
                            break;
                        }
                    }
                }
                i++;
                if ( i == 81) {
                    i = 0;
                }
            }

            int[] outSudoku = new int[81];

            for (int a=0; a<81; a++) {
                if (solvedNums[a] == true)
                {
                    outSudoku[a] = nums[a][0];
                } else {
                    outSudoku[a] = 0;
                }
            }

            return outSudoku;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i = 1;
            int[] order = {
                75,74,73,78,77,76,79,80,81,
                66,65,64,69,68,67,70,71,72,
                57,56,55,60,59,58,61,62,63,
                48,47,46,51,50,49,52,53,54,
                39,38,37,42,41,40,43,44,45,
                30,29,28,33,32,31,34,35,36,
                21,20,19,24,23,22,25,26,27,
                12,11,10,15,14,13,16,17,18,
                3,2,1,6,5,4,7,8,9
            };

            int[] sudoku = new int[81];

            foreach (var textbox in this.Controls.OfType<TextBox>())
            {
                if (Regex.IsMatch(textbox.Text, @"[1-9]"))
                {
                    if (Int32.Parse(textbox.Text) <= 9 && Int32.Parse(textbox.Text) >= 1)
                    {
                        sudoku[order[i - 1] - 1] = Int32.Parse(textbox.Text);
                    }
                    else {
                        sudoku[order[i - 1] - 1] = 0;
                    }
                }
                else {
                    sudoku[order[i - 1] - 1] = 0;
                }
                i++;
            }

            sudoku = solveSudoku(sudoku);

            progressBar1.Value = 100;

            i = 0;

            foreach (var textbox in this.Controls.OfType<TextBox>()) {
                if (sudoku[order[i] - 1].ToString() != "0")
                {
                    textbox.Text = sudoku[order[i] - 1].ToString();
                } else {
                    textbox.Text = "-";
                }
                i++;
            }

            StreamWriter file = new StreamWriter(@"sudoku.txt");

            foreach (var number in sudoku)
            {
                file.WriteLine(number.ToString());
            }

            file.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {
            int[] order = {
                75,74,73,78,77,76,79,80,81,
                66,65,64,69,68,67,70,71,72,
                57,56,55,60,59,58,61,62,63,
                48,47,46,51,50,49,52,53,54,
                39,38,37,42,41,40,43,44,45,
                30,29,28,33,32,31,34,35,36,
                21,20,19,24,23,22,25,26,27,
                12,11,10,15,14,13,16,17,18,
                3,2,1,6,5,4,7,8,9
            };

            string z = File.ReadAllText(Interaction.InputBox("Path: ", "Sudoku solver"));

            int it = 0;

            foreach (var textbox in this.Controls.OfType<TextBox>()) {
                if (z[order[it] - 1] != '-')
                {
                    textbox.Text = z[order[it] - 1].ToString();
                }
                else {
                    textbox.Text = "";
                }
                it++;
            }


        }

        private void Save_Click(object sender, EventArgs e)
        {
            int[] order = {
                75,74,73,78,77,76,79,80,81,
                66,65,64,69,68,67,70,71,72,
                57,56,55,60,59,58,61,62,63,
                48,47,46,51,50,49,52,53,54,
                39,38,37,42,41,40,43,44,45,
                30,29,28,33,32,31,34,35,36,
                21,20,19,24,23,22,25,26,27,
                12,11,10,15,14,13,16,17,18,
                3,2,1,6,5,4,7,8,9
            };

            char[] str = new char[81];

            int it = 0;

            foreach (var textbox in this.Controls.OfType<TextBox>()) {
                if (textbox.Text != "")
                {
                    str[order[it] - 1] = Convert.ToChar(textbox.Text);
                }
                else {
                    str[order[it] - 1] = '-';
                }
                it++;
            }

            File.WriteAllText(Interaction.InputBox("Path: ", "Sudoku solver"), new string(str));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int[] order = {
                75,74,73,78,77,76,79,80,81,
                66,65,64,69,68,67,70,71,72,
                57,56,55,60,59,58,61,62,63,
                48,47,46,51,50,49,52,53,54,
                39,38,37,42,41,40,43,44,45,
                30,29,28,33,32,31,34,35,36,
                21,20,19,24,23,22,25,26,27,
                12,11,10,15,14,13,16,17,18,
                3,2,1,6,5,4,7,8,9
            };

            string z = File.ReadAllText(Interaction.InputBox("Path: ", "Sudoku solver"));

            int it = 0;

            foreach (var textbox in this.Controls.OfType<TextBox>())
            {
                if (z[order[it] - 1] != '-')
                {
                    textbox.Text = z[order[it] - 1].ToString();
                }
                else
                {
                    textbox.Text = "";
                }
                it++;
            }


        }

        private void label1_Click_2(object sender, EventArgs e)
        {

        }
    }
}
