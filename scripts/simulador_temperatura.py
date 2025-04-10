import win32com.client
import time
import random

# Conectar ao servidor OPC da Kepware
opc = win32com.client.Dispatch("Kepware.KEPServerEX.V6")
server = opc.Connect("Kepware.KEPServerEX.V6")

# Loop de simulação
while True:
    temperatura = round(random.uniform(30.0, 35.0), 2)
    server.Write("Simulador.Dorna01.Temperatura", temperatura)
    print(f"Temperatura simulada: {temperatura} °C")
    time.sleep(1)
