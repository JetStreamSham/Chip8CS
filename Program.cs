class Chip8
{
    public Chip8(){
        ram = new byte[4096];
        display = new byte[32*64];
        key = new byte[16];
        stack = new byte[16];
        registers = new byte[16];
    }

    public byte[] ram;
    public byte [] display;
    public byte[] key;
    public short delay;
    public short sound;

    public byte[] stack;
    public byte stackPointer;
    public short programCounter;

    public byte[] registers;

    void Interpret()
    {
        short inst = (short)(ram[programCounter] << 8 | ram[programCounter]);
        switch(inst & 0xF000)
        {
            case 0x0:
            {
                switch(inst & 0x00FF)
                {
                    //clear display
                    case 0xE0:
                    {
                        for(int i = 0; i < display.Length;i++)
                        {
                            display[i]=0;
                        }
                        break;
                    }
                    case 0xEE:
                    {
                        programCounter = stack[stackPointer--];
                        return;
                        break;
                    }
                }
                break;
            }
            case 0x1000:
            {
                programCounter =  (short)(inst&0x0FFF);
                break;
            }
            case 0x2000:
            {
                stack[stackPointer++] =  (byte)(inst & 0x0FFF);
                return;
                break;
            }
            case 0x3000:
            {
                if(registers[inst & 0x0F00] == 0x00FF)
                {
                    programCounter+=2;
                }
                break;
            }
            case 0x4000:
            {
                if(registers[inst & 0x0F00] != 0x00FF)
                {
                    programCounter+=2;
                }
                break;
            }
            case 0x5000:
            {
                if(registers[inst & 0x0F00] == registers[inst & 0x00F0])
                {
                    programCounter+=2;
                }
                break;

            }
            case 0x6000:
            {
                registers[inst & 0x0f00] = (byte)(inst&0x00FF);
                break;
            }
            case 0x7000:
            {

                registers[inst & 0x0f00] += (byte)(inst&0x00FF);
                break;
            }
            case 0x8000:
            {
                switch(inst & 0x000F)
                {
                    case 0x0:
                    {

                        registers[inst & 0x0f00] = registers[inst&0x00F0];
                        break;
                    }
                    case 0x1:
                    {

                        registers[inst & 0x0f00] |= registers[inst&0x00F0];
                        break;
                    }
                    case 0x2:
                    {


                        registers[inst & 0x0f00] &= registers[inst&0x00F0];
                        break;
                    }
                    case 0x3:
                    {

                        registers[inst & 0x0f00] ^= registers[inst&0x00F0];
                        break;
                    }
                    case 0x4:
                    {
                        short xReg = registers[inst & 0x0f00];
                        registers[inst & 0x0f00] += registers[inst&0x00F0];
                        if(xReg < registers[inst & 0x0f00])
                        {
                            registers[0xf] = 1
                        } else{
                            registers[0xf] = 0;
                        }
                        break;
                    }
                    case 0x5:
                    {
                        short xReg = registers[inst & 0x0f00];
                        registers[inst & 0x0f00] -= registers[inst&0x00F0];
                        if(xReg > registers[inst & 0x0f00])
                        {
                            registers[0xf] = 1
                        } else{
                            registers[0xf] = 0;
                        }
                        break;
                    }
                    case 0x6:
                    {
                        short xReg = registers[inst & 0x0f00];
                        byte flag = (byte)((xReg & 0x1000) >> 15);
                        registers[inst & 0x0f00] >>=1;
                        registers[0xf] = flag;
                        break;
                    }
                    case 0x7:
                    {
                        short xReg = registers[inst & 0x0f00];
                        registers[inst & 0x0f00] = registers[inst&0x00F0] - registers[inst & 0x0f00];
                        if(xReg > registers[inst & 0x0f00])
                        {
                            registers[0xf] = 1
                        } else{
                            registers[0xf] = 0;
                        }
                        break;
                    }

                }
                break;
            }
            case 0x9000:
            {

                break;
            }
        }
    }
    void CLS(){}
    void RET(){}
    void JP(){}
}


public static class Program
{
    public static void Main(string[] args)
    {
        byte a = 255;
        byte b = 2;

        a+=b;
        Console.WriteLine("A:{0}\nB:{1}\nA+B:{2}\nunsigned A+B:",a,b,(byte)(a+b));
    }
}
