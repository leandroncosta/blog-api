import 'package:flutter/material.dart';
import 'package:flutter_application/Providers/post_provider.dart';
import 'package:flutter_application/app_widget.dart';
import 'package:provider/provider.dart';

void main() {
  runApp(
    ChangeNotifierProvider(create: (context) => PostProvider(), child: const AppWidget())
  );
}
