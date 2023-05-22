import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;

public class Exercicio1 {
    public static void main(String[] args) {
        Scanner scan = new Scanner(System.in);
        List<Float> salarios = new ArrayList<>();
        List<Integer> filhos = new ArrayList<>();
        while (true){
            float salario =scan.nextFloat();
            if(salario < 0)
                break;
            salarios.add(salario);
            filhos.add(scan.nextInt());
        }

        float mediaSalario = salarios.stream().reduce(0.0f, Float::sum) / salarios.size();
        System.out.printf("Media salarial: %f\n", mediaSalario);

        int mediaFilhos = filhos.stream().reduce(0, Integer::sum) / filhos.size();
        System.out.printf("Media filhos: %d\n", mediaFilhos);

        float maxSalario = salarios.stream().max(Float::compare).get();
        System.out.printf("Salario maximo: %f\n", maxSalario);

        float percentual = salarios.stream().filter(x -> x>1000.0f).count() / (float) salarios.size() * 100;
        System.out.printf("Percentual de pessoas com salario maior que R$1000,00: %f%%", percentual);
    }
}