import 'package:flutter/material.dart';
import 'package:flutter_application/Providers/post_provider.dart';
import 'package:flutter_application/pages/login_page.dart';
import 'package:flutter_application/pages/posts/post_view.dart';
import 'package:flutter_application/pages/user_register/user_register_page.dart';
import 'package:provider/provider.dart';

class AppWidget extends StatelessWidget {
  const AppWidget({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: "${context.read<PostProvider>().isDarktheme}",
      theme: context.watch<PostProvider>().isDarktheme ? ThemeData.dark():ThemeData.light(),
      initialRoute: "/postview",
      routes: {
        "/postview":(context)=> const PostView(),
        "/login": (context) => const LoginPage(),
        "/register": (context) => const UserRegisterPage()
      },
    );
  }
}
