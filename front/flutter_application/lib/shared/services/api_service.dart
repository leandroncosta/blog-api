import 'dart:convert';
import 'dart:developer';
import 'dart:io';
import 'package:http/io_client.dart';
// import 'package:http/http.dart' as http;

class ApiService {
  final String baseUrl = "https://10.0.2.2:7101/api";

  Future<Map<String, dynamic>> createUser(
      String username, String password) async {
    final url = Uri.parse('$baseUrl/users');

    final HttpClient httpClient = HttpClient()
      ..badCertificateCallback =
          (X509Certificate cert, String host, int port) => true;
    final ioClient = IOClient(httpClient);

    final response = await ioClient.post(
      url,
      headers: {'Content-Type': 'application/json'},
      body: json.encode({
        'userName': username,
        'password': password,
      }),
    );
    if (response.statusCode == 201) {
      return json.decode(response.body);
    } else {
      final res = json.decode(response.body);
      log('Error : ${const JsonEncoder.withIndent(" ").convert(res)}');
      log(res["Error"]);
      throw Exception('Falha ao criar usuário');
    }
  }
}
